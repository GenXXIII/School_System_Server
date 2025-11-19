using System.Text.Json;
using Infrastructure.Data;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Repositories;
using Api.Converters;
using Application.Mapping;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyNullableJsonConverter());
        options.JsonSerializerOptions.WriteIndented = true;
    });

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();