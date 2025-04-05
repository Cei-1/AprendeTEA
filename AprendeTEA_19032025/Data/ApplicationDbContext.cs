using AprendeTEA_19032025.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AprendeTEA_19032025.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    public DbSet<Admin> Admin { get; set; }

    public DbSet<Prueba> Prueba { get; set; }
}
