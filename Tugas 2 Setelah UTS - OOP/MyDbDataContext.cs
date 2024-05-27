using Microsoft.EntityFrameworkCore;
using System;
using Tugas2___OOP;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tugas2___OOP
{
    public class MyDbDataContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public MyDbDataContext(DbContextOptions<MyDbDataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(); // Enable sensitive data logging
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .HasMany(b => b.ToDoTasks)
                .WithOne(t => t.ToDoBoard)
                .HasForeignKey(t => t.ToDoBoardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Board>()
                .HasMany(b => b.DoingTasks)
                .WithOne(t => t.DoingBoard)
                .HasForeignKey(t => t.DoingBoardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Board>()
                .HasMany(b => b.DoneTasks)
                .WithOne(t => t.DoneBoard)
                .HasForeignKey(t => t.DoneBoardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Boards)
                .WithOne()
                .HasForeignKey(b => b.UserId);
        }
    }
}