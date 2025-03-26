using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LuxuryLife.Models​;

public partial class TourBookingContext : DbContext
{
    public TourBookingContext()
    {
    }

    public TourBookingContext(DbContextOptions<TourBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerInteraction> CustomerInteractions { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Homestay> Homestays { get; set; }

    public virtual DbSet<Listimagestour> Listimagestours { get; set; }

    public virtual DbSet<MomoPayment> MomoPayments { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderBankInfo> ProviderBankInfos { get; set; }

    public virtual DbSet<ProviderRevenue> ProviderRevenues { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewOn> ReviewOns { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DAMQUOCDAN;Database=TourBooking;uid=sa;pwd=1234;MultipleActiveResultSets=True;TrustServerCertificate=True");

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

            entity.HasIndex(e => e.CustomerId, "IX_Booking_CustomerId");

            entity.HasIndex(e => e.TourId, "IX_Booking_TourId");

            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Booking__Custome__5812160E");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Booking__TourId__59063A47");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259B1A49F1D3");

            entity.ToTable("Contact");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Subject).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Contact__Custome__2FCF1A8A");
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

        modelBuilder.Entity<CustomerInteraction>(entity =>
        {
            entity.HasKey(e => e.InteractionId).HasName("PK__Customer__922C0496A7DD884B");

            entity.ToTable("CustomerInteraction");

            entity.Property(e => e.InteractionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InteractionType).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerInteractions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerI__Custo__2A164134");

            entity.HasOne(d => d.Tour).WithMany(p => p.CustomerInteractions)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__CustomerI__TourI__2B0A656D");
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favourit__CE74FAD5B2B8E927");

            entity.ToTable("Favourite");

            entity.HasIndex(e => e.CustomerId, "IX_Favourite_CustomerId");

            entity.HasIndex(e => e.TourId, "IX_Favourite_TourId");

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

            entity.HasIndex(e => e.TourId, "IX_Homestay_TourId");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PricePerNight).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Tour).WithMany(p => p.Homestays)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Homestay_Tour");
        });

        modelBuilder.Entity<Listimagestour>(entity =>
        {
            entity.HasKey(e => e.ListimagestourId).HasName("PK__Listimag__C13099DB06DC99D8");

            entity.ToTable("Listimagestour");

            entity.HasIndex(e => e.TourId, "IX_Listimagestour_TourId");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageDescription).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(d => d.Tour).WithMany(p => p.Listimagestours)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Listimage__TourI__46E78A0C");
        });

        modelBuilder.Entity<MomoPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__MomoPaym__9B556A384B72CF57");

            entity.ToTable("MomoPayment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.RequestId).HasMaxLength(100);
            entity.Property(e => e.TransactionId).HasMaxLength(100);

            entity.HasOne(d => d.Booking).WithMany(p => p.MomoPayments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__MomoPayme__Booki__3587F3E0");

            entity.HasOne(d => d.Customer).WithMany(p => p.MomoPayments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__MomoPayme__Custo__3493CFA7");
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

        modelBuilder.Entity<ProviderBankInfo>(entity =>
        {
            entity.HasKey(e => e.BankInfoId).HasName("PK__Provider__6E556906E78AC512");

            entity.ToTable("ProviderBankInfo");

            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Qrcode).HasColumnName("QRCode");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderBankInfos)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK_ProviderBank");
        });

        modelBuilder.Entity<ProviderRevenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__Provider__275F16DDBB6CE1DE");

            entity.ToTable("ProviderRevenue");

            entity.HasIndex(e => new { e.ProviderId, e.RevenueMonth, e.RevenueYear }, "UQ_ProviderRevenue").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RevenueAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderRevenues)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK_ProviderRevenue");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE402D27CA");

            entity.ToTable("Review");

            entity.HasIndex(e => e.CustomerId, "IX_Review_CustomerId");

            entity.HasIndex(e => e.TourId, "IX_Review_TourId");

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

        modelBuilder.Entity<ReviewOn>(entity =>
        {
            entity.HasKey(e => e.ReviewOnId).HasName("PK__ReviewOn__2D882DB1A71368C3");

            entity.ToTable("ReviewOn");

            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.ReviewOns)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewOn__Custom__51300E55");

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewOns)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewOn__Review__503BEA1C");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB00AB64FBE5D");

            entity.ToTable("Service");

            entity.HasIndex(e => e.TourId, "IX_Service_TourId");

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

            entity.HasIndex(e => e.ProviderId, "IX_Tour_ProviderId");

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
