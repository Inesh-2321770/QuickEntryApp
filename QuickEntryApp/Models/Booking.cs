using System;
using System.ComponentModel.DataAnnotations;

namespace QuickEntryApp.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        public int VenueID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Payment ID is required")]
        public string PaymentID { get; set; }

        [Required(ErrorMessage = "Toatal Price is required")]
        public float Toatalpayment { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }


        //Navigation property
        public virtual Venue Venue { get; set; }

    }
}
