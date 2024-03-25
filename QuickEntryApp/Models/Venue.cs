using System;
using System.ComponentModel.DataAnnotations;

namespace QuickEntryApp.Models
{
    public class Venue
    {
        [Key]
        public int VenueID { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        public string VenueName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to zero")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
