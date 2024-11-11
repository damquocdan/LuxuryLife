using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LuxuryLife.Models;

public partial class LuxuryLifeContext : DbContext
{
    public LuxuryLifeContext()
    {
    }

    public LuxuryLifeContext(DbContextOptions<LuxuryLifeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingService> BookingServices { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerSupport> CustomerSupports { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Homestay> Homestays { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourPackage> TourPackages { get; set; }

    public virtual DbSet<TourSchedule> TourSchedules { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DAMQUOCDAN;Database=LuxuryLife;uid=sa;pwd=1234;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__AD0500A6D685652C");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__AB6E616411621B64").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("adminId");
            entity.Property(e => e.ActionHistory)
                .HasColumnType("text")
                .HasColumnName("actionHistory");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__C6D03BCDCED18E57");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.BookingDate).HasColumnName("bookingDate");
            entity.Property(e => e.CheckInDate).HasColumnName("checkInDate");
            entity.Property(e => e.CheckOutDate).HasColumnName("checkOutDate");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Booking__custome__778AC167");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Booking__homesta__787EE5A0");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Booking__tourId__797309D9");
        });

        modelBuilder.Entity<BookingService>(entity =>
        {
            entity.HasKey(e => e.BookingServiceId).HasName("PK__BookingS__7D5589588F1C4520");

            entity.ToTable("BookingService");

            entity.Property(e => e.BookingServiceId).HasColumnName("bookingServiceId");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("serviceName");
            entity.Property(e => e.ServicePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("servicePrice");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingServices)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__BookingSe__booki__7A672E12");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__B611CB7DB2D2C5C1");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ__Customer__AB6E6164E8501E1C").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Demographics)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("demographics");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Preferences)
                .HasColumnType("text")
                .HasColumnName("preferences");
            entity.Property(e => e.SearchHistory)
                .HasColumnType("text")
                .HasColumnName("searchHistory");
        });

        modelBuilder.Entity<CustomerSupport>(entity =>
        {
            entity.HasKey(e => e.SupportId).HasName("PK__Customer__E6765E5495BDC4FF");

            entity.ToTable("CustomerSupport");

            entity.Property(e => e.SupportId).HasColumnName("supportId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.IsResolved)
                .HasDefaultValue(false)
                .HasColumnName("isResolved");
            entity.Property(e => e.IssueDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("issueDescription");
            entity.Property(e => e.ResolvedDate)
                .HasColumnType("datetime")
                .HasColumnName("resolvedDate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SupportResponse)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("supportResponse");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerSupports)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerS__custo__7B5B524B");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__876A64D510C3AA39");

            entity.ToTable("Favorite");

            entity.Property(e => e.FavoriteId).HasColumnName("favoriteId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorite__custom__7C4F7684");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.HomestayId)
                .HasConstraintName("FK__Favorite__homest__7D439ABD");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Favorite__tourId__7E37BEF6");
        });

        modelBuilder.Entity<Homestay>(entity =>
        {
            entity.HasKey(e => e.HomestayId).HasName("PK__Homestay__C9DE63037A3866EE");

            entity.ToTable("Homestay");

            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.Availability)
                .HasDefaultValue(true)
                .HasColumnName("availability");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PricePerNight).HasColumnName("pricePerNight");
            entity.Property(e => e.ProviderId).HasColumnName("providerId");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0.0)
                .HasColumnName("rating");

            entity.HasOne(d => d.Provider).WithMany(p => p.Homestays)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Homestay__provid__7F2BE32F");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Image__336E9B551F7B53C3");

            entity.ToTable("Image");

            entity.Property(e => e.ImageId).HasColumnName("imageId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.ImageDescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imageDescription");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imageUrl");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Images)
                .HasForeignKey(d => d.HomestayId)
                .HasConstraintName("FK__Image__homestayI__00200768");

            entity.HasOne(d => d.Tour).WithMany(p => p.Images)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Image__tourId__01142BA1");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__5218041E6FAAB679");

            entity.Property(e => e.NewsId).HasColumnName("newsId");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.ProviderId).HasColumnName("providerId");
            entity.Property(e => e.PublicationDate).HasColumnName("publicationDate");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Author).WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__News__authorId__02084FDA");

            entity.HasOne(d => d.Homestay).WithMany(p => p.News)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__News__homestayId__02FC7413");

            entity.HasOne(d => d.Provider).WithMany(p => p.News)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__News__providerId__03F0984C");

            entity.HasOne(d => d.Tour).WithMany(p => p.News)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__News__tourId__04E4BC85");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__4BA5CEA9C3DE6F62");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.NotificationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("notificationDate");
            entity.Property(e => e.RecipientId).HasColumnName("recipientId");
            entity.Property(e => e.RecipientType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("recipientType");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFC6A4F98E89");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paymentStatus");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Payment__booking__05D8E0BE");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__C7B34C89DD39BACD");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.MethodId).HasColumnName("methodId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.MethodName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("methodName");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__Provider__107017F3DDB9659B");

            entity.ToTable("Provider");

            entity.HasIndex(e => e.Email, "UQ__Provider__AB6E616433288F21").IsUnique();

            entity.Property(e => e.ProviderId).HasColumnName("providerId");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0.0)
                .HasColumnName("rating");
            entity.Property(e => e.Revenue)
                .HasDefaultValue(0.0)
                .HasColumnName("revenue");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__2D290CA99B117CB9");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("ratingId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.RatingValue).HasColumnName("ratingValue");
            entity.Property(e => e.ReviewComment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("reviewComment");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Rating__customer__06CD04F7");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.HomestayId)
                .HasConstraintName("FK__Rating__homestay__07C12930");

            entity.HasOne(d => d.Tour).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Rating__tourId__08B54D69");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__2ECD6E043DA9A97B");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Review__customer__09A971A2");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Review__homestay__0A9D95DB");

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Review__tourId__0B91BA14");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__455070DF13644FD9");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("serviceId");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.HomestayId).HasColumnName("homestayId");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("serviceName");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Homestay).WithMany(p => p.Services)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Service__homesta__0C85DE4D");

            entity.HasOne(d => d.Tour).WithMany(p => p.Services)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Service__tourId__0D7A0286");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK__Tour__519D1D63CF81A39D");

            entity.ToTable("Tour");

            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.AvailableSeats).HasColumnName("availableSeats");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Destination)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("destination");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.MaxParticipants).HasColumnName("maxParticipants");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PricePerPerson).HasColumnName("pricePerPerson");
            entity.Property(e => e.ProviderId).HasColumnName("providerId");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0.0)
                .HasColumnName("rating");
            entity.Property(e => e.StartDate).HasColumnName("startDate");

            entity.HasOne(d => d.Provider).WithMany(p => p.Tours)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Tour__providerId__0E6E26BF");
        });

        modelBuilder.Entity<TourPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__TourPack__AA8D20C87C155396");

            entity.ToTable("TourPackage");

            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("packageName");
            entity.Property(e => e.PackagePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("packagePrice");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourPackages)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__TourPacka__tourI__0F624AF8");
        });

        modelBuilder.Entity<TourSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__TourSche__A532EDD476BC3799");

            entity.ToTable("TourSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");
            entity.Property(e => e.ActivityDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("activityDescription");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.ScheduleDate).HasColumnName("scheduleDate");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourSchedules)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__TourSched__tourI__10566F31");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__9B57CF7279FDFE1B");

            entity.Property(e => e.TransactionId).HasColumnName("transactionId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transactionDate");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transactionStatus");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Transacti__custo__114A936A");

            entity.HasOne(d => d.Payment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Transacti__payme__123EB7A3");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Voucher__F53389E9643B1EB9");

            entity.ToTable("Voucher");

            entity.HasIndex(e => e.Code, "UQ__Voucher__357D4CF9E6FEA4EB").IsUnique();

            entity.Property(e => e.VoucherId).HasColumnName("voucherId");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discountPercentage");
            entity.Property(e => e.MinSpendAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("minSpendAmount");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.ValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("validFrom");
            entity.Property(e => e.ValidUntil)
                .HasColumnType("datetime")
                .HasColumnName("validUntil");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
