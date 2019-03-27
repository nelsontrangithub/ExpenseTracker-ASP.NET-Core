using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpenseController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Expense
        public ActionResult Index()
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            List<Expense> expenses = new List<Expense>();
            ExpenseRepo expRepo = new ExpenseRepo(_db);
            expenses = expRepo.GetAllExpenses().ToList();
            return View(expenses);
        }

        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Expense/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expense/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expense expense)
        {
            ExpenseRepo expRepo = new ExpenseRepo(_db);
            ViewData["UserID"] = new SelectList(_db.ApplicationUsers, "Id", "Id", expense.UserID);
            try
            {
                if (ModelState.IsValid)
                {
                    expRepo.AddExpense(expense);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Expense/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}