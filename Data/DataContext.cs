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

        // public DbSet<Character> Characters => Set<Character>();
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<MessagesModel> Messages => Set<MessagesModel>();
        public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessagesModel>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);
        }
    }
}