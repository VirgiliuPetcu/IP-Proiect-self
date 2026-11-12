using Microsoft.EntityFrameworkCore;
using saxafon.Models;

namespace saxafon.Data
{
    public class AppDbContext : DbContext
    {


        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Resources> Resources { get; set; } = null!;
        public DbSet<Rooms> Rooms { get; set; } = null!;
        public DbSet<Equipments> Equipments { get; set; } = null!;
        public DbSet<Reservations> Reservations { get; set; } = null!;



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false).AddUserSecrets<AppDbContext>().Build();
                var connectionString = builder.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySQL(connectionString);
            
            }
            base.OnConfiguring(optionsBuilder);
        }
         */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resources>().Property(e => e.status).HasConversion(v => v.ToString(), v => (Status)Enum.Parse(typeof(Status), v));
            modelBuilder.Entity<Reservations>().Property(e => e.status_rezervare).HasConversion(v => v.ToString(), v => (ReservationStatus)Enum.Parse(typeof(ReservationStatus), v));
            modelBuilder.Entity<User>().Property(e => e.role).HasConversion(v => v.ToString(), v => (Role)Enum.Parse(typeof(Role), v));
         
            base.OnModelCreating(modelBuilder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Enum>().HaveConversion<string>().HaveMaxLength(25);
            base.ConfigureConventions(configurationBuilder);
        }
       

    }
}
