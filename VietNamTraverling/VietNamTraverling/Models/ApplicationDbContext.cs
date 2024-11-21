using Microsoft.EntityFrameworkCore;

namespace VietNamTraverling.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<ListImageTour> ListImageTours { get; set; }
        public DbSet<Homestay> Homestays { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<News> News { get; set; }

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Using the connection string from appsettings.json
            var connectionString = _configuration.GetConnectionString("VietNamTraverllingConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any relationships or custom configurations here
        }
    }

}
