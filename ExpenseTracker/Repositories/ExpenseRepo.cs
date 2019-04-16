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

        public IQueryable<Expense> GetExpensesBySearch(string userID, string searchString)
        {
            var expenses = _context.Expenses
                .Where(e => e.UserID == userID && e.ExpenseName.ToLower().Contains(searchString.ToLower()) || e.UserID == userID && e.Category.ToLower().Contains(searchString.ToLower()));
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

        public IQueryable<Expense> SearchExpensesWithCategory(string userID, string category, string searchString)
        {
            var expenses = _context.Expenses
                .Where(e => e.UserID == userID && e.Category == category && e.ExpenseName.ToLower().Contains(searchString.ToLower()));
            return expenses;
        }

        public decimal CalculateCategoryTotal(string category, string userID)
        {
            decimal catSum = _context.Expenses.Where
            (cat => cat.Category == category && cat.UserID == userID)
            .Select(cat => cat.Amount)
            .Sum();

            return catSum;
        }

        public decimal CalculateWeeklyTotal(string category, string userID)
        {
            decimal total = _context.Expenses.Where
                (cat => cat.Category == category && cat.UserID == userID && cat.ExpenseDate > DateTime.Now.AddDays(7))
                .Select(cat => cat.Amount)
                .Sum();

            return total;
        }

        public IQueryable<Expense> SortExpenses(IQueryable<Expense> expenses, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    expenses = expenses.OrderByDescending(s => s.ExpenseName);
                    break;
                case "Date":
                    expenses = expenses.OrderBy(s => s.ExpenseDate);
                    break;
                case "date_desc":
                    expenses = expenses.OrderByDescending(s => s.ExpenseDate);
                    break;
                case "Amount":
                    expenses = expenses.OrderBy(s => s.Amount);
                    break;
                case "amount_desc":
                    expenses = expenses.OrderByDescending(s => s.Amount);
                    break;
                case "Category":
                    expenses = expenses.OrderBy(s => s.Category);
                    break;
                case "category_desc":
                    expenses = expenses.OrderByDescending(s => s.Category);
                    break;
                default:
                    expenses = expenses.OrderBy(s => s.ExpenseName);
                    break;
            }
            return expenses;
        }
    }
}
