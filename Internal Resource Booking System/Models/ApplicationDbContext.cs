using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Internal_Resource_Booking_System.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=InternalResourceBookingDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3214EC07D2D5DE1E");

            entity.Property(e => e.BookedBy).HasMaxLength(100);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Purpose).HasMaxLength(255);
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Resource).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ResourceId)
                .HasConstraintName("FK_Bookings_Resources");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resource__3214EC079AA7BE7F");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
