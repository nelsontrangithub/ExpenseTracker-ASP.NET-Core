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
        private ApplicationDbContext db;

        public ExpenseRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Expense> GetAllExpenses()
        {
            try
            {
                return db.Expenses.ToList();
            }
            catch
            {
                throw;
            }
        }

        //Add      
        public void AddExpense(Expense expense)
        {
            try
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        //Update 
        public int UpdateExpense(Expense expense)
        {
            try
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }
        //Get by ID  
        public Expense GetExpenseData(int id)
        {
            try
            {
                Expense expense = db.Expenses.Find(id);
                return expense;
            }
            catch
            {
                throw;
            }
        }
        //Delete 
        public void DeleteExpense(int id)
        {
            try
            {
                Expense exp = db.Expenses.Find(id);
                db.Expenses.Remove(exp);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
