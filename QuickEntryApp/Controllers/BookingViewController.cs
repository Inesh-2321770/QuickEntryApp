using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickEntryApp.Data;
using QuickEntryApp.Models;
using QuickEntryApp.Utilities;


namespace QuickEntryApp.Controllers
{
    public class BookingViewController : Controller
    {
        private readonly QuickEntryAppDbContext _context;
        private const int PageSize = 5;

        public BookingViewController(QuickEntryAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BookingView(string searchString, int? pageNumber)
        {
            var bookings = _context.Bookings.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => b.UserName.Contains(searchString) 
                                            || b.Venue.VenueName.Contains(searchString)
                                            || b.Status.Contains(searchString));
            }

            bookings = bookings.Include(b => b.Venue);

            return View(await PaginatedList<Booking>.CreateAsync(bookings.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        public IActionResult RedirectAdmin()
        {
            return RedirectToAction("AdminDashboard","AdminDashboard");
        }

    }
}
