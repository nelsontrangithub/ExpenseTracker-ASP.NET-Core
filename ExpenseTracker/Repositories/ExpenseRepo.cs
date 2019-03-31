using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepo
    {
        private ApplicationDbContext _context;

        public ExpenseRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Expense> GetAllExpenses(string userID)
        {
            var expenses = _context.Expenses.Where(e => e.UserID == userID);
            return expenses;
        }

        public Expense GetById(int? id)
        {
            var expense = _context.Expenses.Include(e => e.ApplicationUser).FirstOrDefault(m => m.Id == id);
            return expense;
        }

        public IQueryable<Expense> GetExpensesByCategory(string userID, string category)
        {
            var expenses = _context.Expenses.Where(e => e.UserID == userID && e.Category == category);
            return expenses;
        }

        public decimal CalculateMonthlyExpense(string category, string userID)
        {
            decimal catSum = _context.Expenses.Where
            (cat => cat.Category == category && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)) && cat.UserID == userID)
            .Select(cat => cat.Amount)
            .Sum();

            return catSum;
        }
    }
}
