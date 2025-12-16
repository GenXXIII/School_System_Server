using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    /*{inherit option from EFCore}*/
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    /*{ Create Table }*/
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<User> Users => Set<User>();

}