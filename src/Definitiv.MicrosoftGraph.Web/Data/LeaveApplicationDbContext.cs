using Microsoft.EntityFrameworkCore;

namespace Definitiv.MicrosoftGraph.Web.Data
{
    public class LeaveApplicationDbContext : DbContext
    {
        public LeaveApplicationDbContext(
            DbContextOptions<LeaveApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LeaveApplication> LeaveApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany<LeaveApplication>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmailAddress)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasData(
                    new Employee
                    {
                        Id = new Guid("{C7CA12D8-7AA9-49AB-9511-A62CDC391632}"),
                        EmailAddress = "tony.stark@HackathonOct22Definitiv.onmicrosoft.com"
                    },

                    new Employee
                    {
                        Id = new Guid("{97AC4C3D-4FE9-4417-9B2D-979257906716}"),
                        EmailAddress = "peter.parker@HackathonOct22Definitiv.onmicrosoft.com"
                    });

            modelBuilder.Entity<LeaveApplication>()
                .HasKey(la => new { la.Id, la.EmployeeId });

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Employee
    {
        public Guid Id { get; set; }

        public string EmailAddress { get; set; }
    }

    public enum LeaveType
    {
        AnnualLeave,
        SickLeave
    }

    public class LeaveApplication
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EmployeeId { get; set; }

        public LeaveType LeaveType { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
        public string? OutlookEventId { get; set; }
    }
}
