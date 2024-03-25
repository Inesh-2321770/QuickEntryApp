using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickEntryApp.Data;
using QuickEntryApp.Models;
using QuickEntryApp.Utilities;

namespace QuickEntryApp.Controllers
{

    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly QuickEntryAppDbContext _context;
        private const int PageSize = 1;

        public AdminDashboardController(QuickEntryAppDbContext context)
        {
            _context = context;
        }


      
        public IActionResult BookingView()
        {
            return View("BookingView");
        }


       


        [HttpPost]
        public ActionResult AddVenue(Venue venue)
        {
            if (!ModelState.IsValid)
            {
                _context.Venues.Add(venue);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Redirect to a view after adding
            }

            return View("AdminDashboard"); // Return back to the form if the model is not valid
        }

       

   
      
        public async Task<IActionResult> AdminDashboard(string searchString, int? pageNumber)
        {
            var venues = from v in _context.Venues
                         select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v => v.VenueName.Contains(searchString)
                                       || v.City.Contains(searchString));
            }

            return View(await PaginatedList<Venue>.CreateAsync(venues.AsNoTracking(), pageNumber ?? 1, PageSize));
        }



       



        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueID,VenueName,Capacity,Address,City,Description,Phone,Price,StartTime,EndTime")] Venue venue)
        {
            if (id != venue.VenueID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminDashboard));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminDashboard));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueID == id);
        }



    }
}

