using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Models;

namespace PayrollSystem.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<WorkSchedule> WorkSchedules { get; set; }

        public DbSet<WorkScheduleDay> WorkScheduleDays { get; set; }
        public DbSet<OvertimeRequest> OvertimeRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.Description)
                    .HasMaxLength(500);

                entity.HasIndex(d => d.Name)
                    .IsUnique();
            });

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.EmployeeNumber)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(30);

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BasicSalary)
                    .HasPrecision(18, 2);

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.WorkSchedule)
                    .WithMany(w => w.Employees)
                    .HasForeignKey(e => e.WorkScheduleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Refresh token configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(r => r.Token)
                    .IsUnique();

                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Attendance configuration
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.HoursWorked)
                    .HasPrecision(5, 2);

                entity.Property(a => a.Notes)
                    .HasMaxLength(500);

                entity.HasIndex(a => new
                {
                    a.EmployeeId,
                    a.AttendanceDate
                })
                .IsUnique();

                entity.HasOne(a => a.Employee)
                    .WithMany()
                    .HasForeignKey(a => a.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // WorkSchedule configuration
            modelBuilder.Entity<WorkSchedule>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.Property(w => w.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(w => w.Description)
                    .HasMaxLength(500);
            });

            // WorkScheduleDay configuration
            modelBuilder.Entity<WorkScheduleDay>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.HasIndex(w => new
                {
                    w.WorkScheduleId,
                    w.DayOfWeek
                })
                .IsUnique();

                entity.HasOne(w => w.WorkSchedule)
                    .WithMany(w => w.Days)
                    .HasForeignKey(w => w.WorkScheduleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OvertimeRequest configuration
            modelBuilder.Entity<OvertimeRequest>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.RequestedHours)
                    .HasPrecision(5, 2);

                entity.Property(o => o.ApprovedHours)
                    .HasPrecision(5, 2);

                entity.Property(o => o.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(o => o.RejectionReason)
                    .HasMaxLength(500);

                entity.HasOne(o => o.Employee)
                    .WithMany()
                    .HasForeignKey(o => o.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Attendance)
                    .WithMany()
                    .HasForeignKey(o => o.AttendanceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(o => new
                {
                    o.EmployeeId,
                    o.AttendanceId
                })
                .IsUnique();
            });
        }
    }
}
