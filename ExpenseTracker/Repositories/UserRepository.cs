using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            this._context = context;
        }

        // Get all users in the databaFse.
        public IEnumerable<UserVM> All()
        {
            var users = _context.Users.Select(u => new UserVM()
            {
                Email = u.Email
            });
            return users;
        }

        public bool Remove(string userName)
        {
            var item = _context.Users.FirstOrDefault(x => x.Email == userName);
            if (item != null)
            {
                _context.Users.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }

}
