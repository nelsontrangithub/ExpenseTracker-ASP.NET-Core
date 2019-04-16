using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.Security.Claims;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public IActionResult Index(string sortOrder, string searchString, int? page)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            var userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string sort = String.IsNullOrEmpty(sortOrder) ? "title_asc" : sortOrder;
            string search = String.IsNullOrEmpty(searchString) ? "" : searchString;
            ViewData["CurrentSort"] = sort;
            ViewData["CurrentFilter"] = search;

            ExpenseRepo expRepo = new ExpenseRepo(_context);

            var expenses = expRepo.GetAllExpenses(userID);

            if (!String.IsNullOrEmpty(searchString))
            {
                expenses = expRepo.GetExpensesBySearch(userID, searchString);
            }
            expenses = expRepo.SortExpenses(expenses, sortOrder);
            int pageSize = 3;
            //return View(await expenses.ToListAsync());
            return View(PaginatedList<Expense>.Create(expenses, page ?? 1, pageSize));
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            //ViewData["UserID"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["UserID"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,ExpenseName,Amount,ExpenseDate,Category")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUsers, "Id", "Id", expense.UserID);
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUsers, "Id", "Id", expense.UserID);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,ExpenseName,Amount,ExpenseDate,Category")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUsers, "Id", "Id", expense.UserID);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        public IActionResult GetCategory(string category, string sortOrder, string searchString, int? page)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            var userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ExpenseRepo expRepo = new ExpenseRepo(_context);
            ViewData["Category"] = category;
            ViewData["Total"] = expRepo.CalculateCategoryTotal(category, userID).ToString("C");
            ViewData["WeeklyTotal"] = expRepo.CalculateWeeklyTotal(category, userID).ToString("C");
            var expenses = expRepo.GetExpensesByCategory(userID, category);

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            string sort = String.IsNullOrEmpty(sortOrder) ? "title_asc" : sortOrder;
            string search = String.IsNullOrEmpty(searchString) ? "" : searchString;
            ViewData["CurrentSort"] = sort;
            ViewData["CurrentFilter"] = search;

            if (!String.IsNullOrEmpty(searchString))
            {
                expenses = expRepo.SearchExpensesWithCategory(userID, category, searchString);
            }
            expenses = expRepo.SortExpenses(expenses, sortOrder);
            int pageSize = 3;
            return View(PaginatedList<Expense>.Create(expenses, page ?? 1, pageSize));
        }
    }
}
