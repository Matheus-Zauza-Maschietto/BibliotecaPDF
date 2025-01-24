using bibliotecaPDF.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<PdfFile> PdfFile { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasOne<CapacityPlan>(p => p.CapacityPlan)
            .WithMany(p => p.Users);
        
        builder.Entity<User>()
            .HasMany<PdfFile>(p => p.Files)
            .WithOne(p => p.User);
        
        builder.Entity<PdfFile>()
            .HasOne(p => p.User)
            .WithMany(p => p.Files);

        builder.Entity<CapacityPlan>()
            .HasMany<User>(p => p.Users)
            .WithOne(p => p.CapacityPlan);
        
        builder.Entity<CapacityPlan>()
            .HasData(new CapacityPlan()
                {
                    Id = 1,
                    BytesCapacity = 52428800,
                    PlanName = "Plano Gratuito",
                    Value = 0
                },
                new CapacityPlan()
                {
                    Id = 2,
                    BytesCapacity = 1073741824,
                    PlanName = "Plano Inicial",
                    Value = 5
                },
                new CapacityPlan()
                {
                    Id = 3,
                    BytesCapacity = 10737418240,
                    PlanName = "Plano Intermediario",
                    Value = 20
                },
                new CapacityPlan()
                {
                    Id = 4,
                    BytesCapacity = 53687091200,
                    PlanName = "Plano Executivo",
                    Value = 50
                },
                new CapacityPlan()
                {
                    Id = 5,
                    BytesCapacity = 1099511627776,
                    PlanName = "Plano Admin",
                    Value = 1000
                }
            );
    }
}
