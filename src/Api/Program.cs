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

var builder = WebApplication.CreateBuilder(args);

// ========== SERILOG ========== //
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // read from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
// ============================= //

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "School System API",
        Version = "v1"
    });
    c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepositories, StudentRepositories>();
builder.Services.AddScoped<IStudentServices, StudentServices>();
builder.Services.AddScoped<ITeacherRepositories, TeacherRepositories>();
builder.Services.AddScoped<ITeacherServices, TeacherServices>();
builder.Services.AddScoped<ICourseRepositories, CourseRepositories>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<IClassroomRepositories, ClassroomRepositories>();
builder.Services.AddScoped<IClassroomServices, ClassroomServices>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
