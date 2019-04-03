using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            string adminID = "591c5c95-edc5-4ec8-ba0a-bf2cecac8f9e";
            var query = context.Expenses.Where(e => e.UserID == adminID).FirstOrDefault();

            if (query != null)
            {
                return;   // DB has been seeded
            }

            var expenses = new Expense[]
            {
                new Expense
                {
                    UserID = "591c5c95-edc5-4ec8-ba0a-bf2cecac8f9e",
                    ExpenseName = "Coffee",
                    ExpenseDate = DateTime.Now,
                    Amount = 5.99m,
                    Category = "Food"
                },
                new Expense
                {
                    UserID = "591c5c95-edc5-4ec8-ba0a-bf2cecac8f9e",
                    ExpenseName = "Rent",
                    ExpenseDate = DateTime.Now,
                    Amount = 1000.99m,
                    Category = "Housing"
                },
                new Expense
                {
                    UserID = "591c5c95-edc5-4ec8-ba0a-bf2cecac8f9e",
                    ExpenseName = "Netflix",
                    ExpenseDate = DateTime.Now,
                    Amount = 10.99m,
                    Category = "Pleasure"
                },
                new Expense
                {
                    UserID = "591c5c95-edc5-4ec8-ba0a-bf2cecac8f9e",
                    ExpenseName = "Electricity",
                    ExpenseDate = DateTime.Now,
                    Amount = 100.99m,
                    Category = "Utilities"
                },
            };

            foreach (Expense e in expenses)
            {
                context.Expenses.Add(e);
            }
            context.SaveChanges();
        }
    }
}
