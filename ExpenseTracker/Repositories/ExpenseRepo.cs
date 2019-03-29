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

    }
}
