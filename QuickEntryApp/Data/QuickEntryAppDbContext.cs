using Microsoft.EntityFrameworkCore;
using QuickEntryApp.Models;

namespace QuickEntryApp.Data
{
    public class QuickEntryAppDbContext : DbContext
    {
        public QuickEntryAppDbContext(DbContextOptions<QuickEntryAppDbContext>options) : base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }  
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}
