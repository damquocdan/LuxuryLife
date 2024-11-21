using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LuxuryLife.Models​;

public partial class TourbookingContext : DbContext
{
    public TourbookingContext()
    {
    }

    public TourbookingContext(DbContextOptions<TourbookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Homestay> Homestays { get; set; }

    public virtual DbSet<Listimagestour> Listimagestours { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DAMQUOCDAN;Database=tourbooking;uid=sa;pwd=1234;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE488CCBBC051");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D10534B027E995").IsUnique();

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AED39332FF9");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Booking__Custome__5812160E");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Booking__TourId__59063A47");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8CED30739");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105343E5F5B4C").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Demographics).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Preferences).HasMaxLength(255);
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favourit__CE74FAD5B2B8E927");

            entity.ToTable("Favourite");

            entity.Property(e => e.Sumprice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Favourite__Custo__4E88ABD4");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Favourite__TourI__4F7CD00D");
        });

        modelBuilder.Entity<Homestay>(entity =>
        {
            entity.HasKey(e => e.HomestayId).HasName("PK__Homestay__EDCB5CBABC26500E");

            entity.ToTable("Homestay");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PricePerNight).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Listimagestour>(entity =>
        {
            entity.HasKey(e => e.ListimagestourId).HasName("PK__Listimag__C13099DB06DC99D8");

            entity.ToTable("Listimagestour");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageDescription).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(d => d.Tour).WithMany(p => p.Listimagestours)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Listimage__TourI__46E78A0C");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewId).HasName("PK__News__7CC3777EEB9A1D67");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__Provider__B54C687D284498A8");

            entity.ToTable("Provider");

            entity.HasIndex(e => e.Email, "UQ__Provider__A9D10534A5ED99BC").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE402D27CA");

            entity.ToTable("Review");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Review__Customer__534D60F1");

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Review__TourId__5441852A");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB00AB64FBE5D");

            entity.ToTable("Service");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);

            entity.HasOne(d => d.Tour).WithMany(p => p.Services)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Service__TourId__4BAC3F29");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK__Tour__604CEA3002475256");

            entity.ToTable("Tour");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PricePerson).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Provider).WithMany(p => p.Tours)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Tour__ProviderId__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
