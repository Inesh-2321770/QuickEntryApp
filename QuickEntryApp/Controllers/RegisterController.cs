using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickEntryApp.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Scripting;
using QuickEntryApp.Data;

namespace QuickEntryApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly QuickEntryAppDbContext _context;

        public RegisterController(QuickEntryAppDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the RegisterViewModel to a User model
                var user = new User
                {
                    Username = model.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    IsAdmin = false
                };

                // Add the user to the database
                _context.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


    }
}
