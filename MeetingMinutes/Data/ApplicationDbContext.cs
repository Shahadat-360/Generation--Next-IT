using Microsoft.EntityFrameworkCore;
using MeetingMinutes.Models;

namespace MeetingMinutes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CorporateCustomer> CorporateCustomers { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
        public DbSet<ProductService> ProductServices { get; set; }
        public DbSet<MeetingMinutesMaster> MeetingMinutesMasters { get; set; }
        public DbSet<MeetingMinutesDetail> MeetingMinutesDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table names to match the requirement
            modelBuilder.Entity<CorporateCustomer>().ToTable("Corporate_Customer_Tbl");
            modelBuilder.Entity<IndividualCustomer>().ToTable("Individual_Customer_Tbl");
            modelBuilder.Entity<ProductService>().ToTable("Products_Service_Tbl");
            modelBuilder.Entity<MeetingMinutesMaster>().ToTable("Meeting_Minutes_Master_Tbl");
            modelBuilder.Entity<MeetingMinutesDetail>().ToTable("Meeting_Minutes_Details_Tbl");

            // Configure relationships
            modelBuilder.Entity<MeetingMinutesDetail>()
                .HasOne<MeetingMinutesMaster>()
                .WithMany(m => m.Details)
                .HasForeignKey(d => d.MeetingMinutesMasterId);
        }
    }
} 