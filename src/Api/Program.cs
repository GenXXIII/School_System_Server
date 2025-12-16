using System.Security.Claims;
using Infrastructure.Data;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Repositories;
using Application.Mapping;
using Application.Validators.Classroom;
using Application.Validators.Course;
using Application.Validators.Student;
using Application.Validators.Teacher;
using Mapster;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Middleware;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
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
    c.AddSecurityDefinition("Role Management", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Token Role"
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
builder.Services.AddScoped<IClassroomRepositories, ClassroomRepositories>();
builder.Services.AddScoped<IClassroomServices, ClassroomServices>();
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<PasswordService>();
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
builder.Services.AddValidatorsFromAssembly(typeof(ClassroomCreateValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ClassroomUpdateValidator).Assembly);

var app = builder.Build();

// Add Note of Url

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"🚀 Listening on {url}");
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
