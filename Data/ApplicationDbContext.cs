using HelmoBilite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelmoBilite.Data;

public class ApplicationDbContext : IdentityDbContext<BasicUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BasicUser>()
            .HasDiscriminator<string>("Account_Type")
            .HasValue<Client>("client")
            .HasValue<Dispa>("dispatcher")
            .HasValue<Driver>("driver");
        
        
        
    }
    
    public DbSet<BasicUser> BasicUsers { get; set; }

    public DbSet<Truck> Trucks { get; set; }
    
    public DbSet<Delivery> Deliveries { get; set; }
    

    public DbSet<HelmoBilite.Models.Client>? Client { get; set; }

    public DbSet<HelmoBilite.Models.Dispa>? Dispa { get; set; }

    public DbSet<HelmoBilite.Models.Driver>? Driver { get; set; }

}