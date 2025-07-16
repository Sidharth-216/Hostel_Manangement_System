using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HostelManagementSystem.Models;

public class HostelDbContext : IdentityDbContext<AppUser>
{
    public HostelDbContext(DbContextOptions<HostelDbContext> options) : base(options) { }

    public DbSet<Allocation> Allocations { get; set; }
    public DbSet<HostelApplication> Applications { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<HostelBlock> HostelBlocks { get; set; }
    public DbSet<Fees> Fees { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ✅ Correct foreign key property name
        builder.Entity<Allocation>()
            .HasOne(a => a.Room)
            .WithMany()
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // If you want to configure other relationships later, add them here
    }
}
