using comebackapi.Models;
using Microsoft.EntityFrameworkCore;

namespace comebackapi.Infrastructure;

public class AppDbContext :DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<TovarAudit> TovarAudits { get; set; }
    
    public DbSet<Tovar> Tovars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TovarAudit>()
            .HasOne(ta => ta.Tovar)
            .WithMany(ta=>ta.Audits) 
            .HasForeignKey(ta => ta.TovarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}