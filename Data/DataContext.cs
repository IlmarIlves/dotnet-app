using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_app.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<MessagesModel> Messages => Set<MessagesModel>();
        public DbSet<CalendarEventModel> CalendarEvents => Set<CalendarEventModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define many-to-many relationship between User and CalendarEvent models
            modelBuilder.Entity<UserModel>()
           .HasMany(u => u.Messages)
           .WithMany(m => m.Users)
           .UsingEntity(j => j.ToTable("UserMessages"));

            // Many-to-many relationship between UserModel and CalendarEventModel
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.CalendarEvents)
                .WithMany(e => e.Users)
                .UsingEntity(j => j.ToTable("UserCalendarEvents"));
        }
    }
}