namespace QuickEntryApp.Models
{
    public class AdminDashboardViewModel
    {
        public AdminDashboardViewModel()
        {
            Bookings = new List<Booking>();
        }

        public QuickEntryApp.Utilities.PaginatedList<Venue> Venues { get; set; }

        public List<Booking> Bookings { get; set; } 
    }
}
