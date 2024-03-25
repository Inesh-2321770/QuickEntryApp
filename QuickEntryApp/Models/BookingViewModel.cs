using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickEntryApp.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Number of visitors is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of visitors must be at least 1.")]
        public int NoOfVisitors { get; set; }

        [Required(ErrorMessage = "Venue selection is required.")]
        [Display(Name = "Select Venue")]
        public int SelectedVenueId { get; set; }

        public List<Venue> Venues { get; set; }

        [Display(Name = "Total Payment")]
        public float TotalPayment { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }


        [Display(Name = "Venue Start Time")]
        public DateTime? VenueStartTime { get; set; }

        [Display(Name = "Venue End Time")]
        public DateTime? VenueEndTime { get; set; }

        [Required(ErrorMessage = "Booking time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Booking Time")]
        public DateTime BookingTime { get; set; }
    }
}