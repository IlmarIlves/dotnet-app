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
        public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessagesModel>()
           .HasOne(m => m.Sender)
           .WithMany(u => u.SentMessages)
           .HasForeignKey(m => m.SenderId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessagesModel>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}