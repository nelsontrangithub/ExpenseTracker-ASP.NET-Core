﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }
        public string UserID { get; set; }
        public DateTime Created { get; set; }
        public decimal Total { get; set; }

        // Parent.
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Really important.

            //---------------------------------------------------------------
            // Define composite primary keys.
            //---------------------------------------------------------------
            // Define foreign keys here. Do not use foreign key annotations.
            modelBuilder.Entity<Invoice>()
                .HasOne(au => au.ApplicationUser) // Parent
                .WithMany(i => i.Invoices) // Child
                .HasForeignKey(fk => new { fk.UserID })
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}
