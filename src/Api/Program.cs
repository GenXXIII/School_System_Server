using System.Security.Claims;
using Infrastructure.Data;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Repositories;
using Application.Mapping;
using Application.Validators.Department;
using Application.Validators.Course;
using Application.Validators.Student;
using Application.Validators.Teacher;
using Mapster;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using NpgsqlTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Serilog
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
    {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
    {"timestamp", new UtcTimestampColumnWriter(NpgsqlDbType.TimestampTz) },
    {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
    {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "logs",
        columnOptions: columnWriters,
        needAutoCreateTable: true,
        batchSizeLimit: 1,
        period: TimeSpan.FromSeconds(1))
    .CreateLogger();

builder.Host.UseSerilog();

// Controller

builder.Services.AddControllers();

// Swagger Doc

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School System API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter Role Token :"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Connection Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Interface Using

builder.Services.AddScoped<IStudentRepositories, StudentRepositories>();
builder.Services.AddScoped<IStudentServices, StudentServices>();
builder.Services.AddScoped<ITeacherRepositories, TeacherRepositories>();
builder.Services.AddScoped<ITeacherServices, TeacherServices>();
builder.Services.AddScoped<ICourseRepositories, CourseRepositories>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<IDepartmentRepositories, DepartmentRepositories>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IUserService, UserService>();

// Mapster

TypeAdapterConfig.GlobalSettings.Scan(typeof(StudentMappingConfig).Assembly);

// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Jwt Security

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

// Validators

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(StudentCreateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(StudentUpdateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(TeacherCreateValidator).Assembly); 
builder.Services.AddValidatorsFromAssembly(typeof(TeacherUpdateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CourseCreateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CourseUpdateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(DepartmentCreateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(DepartmentUpdateValidator).Assembly);

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var passwordService = services.GetRequiredService<PasswordService>();
        await context.Database.MigrateAsync();
        await DbSeeder.SeedAsync(context, passwordService);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Add Note of Url

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"🚀 Listening on {url}/swagger");
    }
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowReactApp");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public class UtcTimestampColumnWriter : ColumnWriterBase
{
    public UtcTimestampColumnWriter(NpgsqlDbType dbType = NpgsqlDbType.TimestampTz) : base(dbType)
    {
    }

    public override object GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        return logEvent.Timestamp.ToUniversalTime();
    }
}
