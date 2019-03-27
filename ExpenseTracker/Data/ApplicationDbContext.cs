using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Really important.

            //---------------------------------------------------------------
            // Define composite primary keys.
            //---------------------------------------------------------------
            // Define foreign keys here. Do not use foreign key annotations.
            modelBuilder.Entity<Expense>()
                .HasOne(au => au.ApplicationUser) // Parent
                .WithMany(i => i.Expenses) // Child
                .HasForeignKey(fk => new { fk.UserID })
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}
