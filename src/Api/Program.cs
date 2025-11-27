using Infrastructure.Data;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Repositories;
using Api.Converters;
using Application.Mapping;
using Application.Validators.Classroom;
using Application.Validators.Course;
using Application.Validators.Student;
using Application.Validators.Teacher;
using Mapster;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

/* { Date Json Converter } */

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyNullableJsonConverter());
        options.JsonSerializerOptions.WriteIndented = true;
    });

/* { Swagger Configuration } */

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

/* { Connecting DB } */

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

/* { Interface connecting } */

builder.Services.AddScoped<IStudentRepositories, StudentRepositories>();
builder.Services.AddScoped<IStudentServices, StudentServices>();
builder.Services.AddScoped<ITeacherRepositories, TeacherRepositories>();
builder.Services.AddScoped<ITeacherServices, TeacherServices>();
builder.Services.AddScoped<ICourseRepositories, CourseRepositories>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<IClassroomRepositories, ClassroomRepositories>();
builder.Services.AddScoped<IClassroomServices, ClassroomServices>();

/* { Mapster configuration } */

TypeAdapterConfig.GlobalSettings.Scan(typeof(StudentMappingConfig).Assembly);
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

/* { Validation Configuration } */

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
/* { Cors } */
app.UseCors("AllowReactApp");

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();