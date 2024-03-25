using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuickEntryApp.Data;
using QuickEntryApp.Models;
using QuickEntryApp.Properties;
using QuickEntryApp.Views.Booking;
using Stripe.Checkout;
using Stripe;
using System.Diagnostics;
using QuickEntryApp.Migrations;
using System.Text.Json;

namespace QuickEntryApp.Controllers
{
    public class BookingFormController : Controller
    {
        private readonly QuickEntryAppDbContext _context;
        private readonly StripeSettings _stripeSettings;
        public BookingFormController(QuickEntryAppDbContext context,IOptions<StripeSettings>stripeSettings)
        {
            _context = context;
            _stripeSettings=stripeSettings.Value; ;
        }

        [HttpGet]
        public IActionResult GetVenuePrice(int venueId)
        {
            var venue = _context.Venues.Find(venueId);
            if (venue != null)
            {
                return Json(venue.Price);
            }
            return Json(0); //Return 0 if venue not found
        }


        [HttpGet]
        public JsonResult GetVenueTimes(int venueId)
        {
            var venue = _context.Venues.Find(venueId);

            if (venue == null)
            {
                return Json(new { success = false });
            }

            var result = new
            {
                success = true,
                startTime = venue.StartTime.ToString("hh\\:mm"),
                endTime = venue.EndTime.ToString("hh\\:mm")
            };

            return Json(result);
        }



        [HttpGet]
        public IActionResult BookingForm()
        {
            var viewModel = new BookingViewModel()
            {
                Venues = _context.Venues.ToList()
            };
            return View(viewModel);
        }



        [HttpPost]
        public IActionResult SubmitBooking(BookingViewModel model)
        {

            HttpContext.Session.SetString("BookingData", JsonSerializer.Serialize(model));
            //add payment gateway code
            // return RedirectToAction("success", model); //temppp
            var currency = "usd"; // Change currency to INR
            var successUrl = $"https://localhost:44351/BookingForm/Success?sessionId={{CHECKOUT_SESSION_ID}}";
            var cancelUrl = $"https://localhost:44351/BookingForm/Cancel?sessionId={{CHECKOUT_SESSION_ID}}";
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
        {
            "card"
        },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = currency,
                    UnitAmountDecimal = Convert.ToInt32(model.TotalPayment)*100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = model.Name,
                        Description = $"Tickets: {model.NoOfVisitors}",
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                CustomerEmail = model.Email,
            };
            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);
            if (session != null)
            {
                // Create a new Booking object
                var booking = new Booking
                {
                    VenueID = model.SelectedVenueId,
                    UserName = model.Name,
                    Date = DateTime.Now, // replace with your actual date
                    Time = TimeSpan.FromHours(10), // replace with your actual time
                    PaymentID = session.Id,
                    Toatalpayment = model.TotalPayment,
                    Status = "Pending" ,// replace with your actual status
                     Venue = _context.Venues.Find(model.SelectedVenueId)

                };

                // Save the Booking object to the database
                _context.Bookings.Add(booking);
                _context.SaveChanges();
            }

            return Redirect(session.Url);
        }






        public async Task<IActionResult> success(BookingViewModel model, string sessionId)
        {

            var booking = _context.Bookings.FirstOrDefault(b => b.PaymentID == sessionId);
            if (booking != null)
            {
                booking.Status = "Paid";
                _context.SaveChanges();
            }
            /* var token = HttpContext.Session.GetString("Token");
             if (string.IsNullOrEmpty(token))
             {
                 // If no token, redirect to home
                 return RedirectToAction("Index", "Home");
             }
            */
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQRCode()
        {
            // Get the booking details from the model
            var bookingJson = HttpContext.Session.GetString("BookingData");
            var booking = JsonSerializer.Deserialize<BookingViewModel>(bookingJson);

            // Prepare the data for the QR code
            // var qrCodeData = $"Booking Name:{booking.Name},Booking Email:{booking.Email},Payment: {booking.TotalPayment},NoOfTickets: {booking.NoOfVisitors}}";
            var qrCodeData = $"Booking Name:{booking.Name},Booking Email:{booking.Email},Payment: {booking.TotalPayment},NoOfTickets: {booking.NoOfVisitors},Date: {booking.BookingDate},Time: {booking.BookingTime}";
            // URL-encode the data
            var encodedData = Uri.EscapeDataString(qrCodeData);

            // Call the QR code API
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://quickchart.io/qr?text={encodedData}&size=250x250&format=png");

            if (response.IsSuccessStatusCode)
            {
                // Get the QR code as a base64 string
                var qrCodeBytes = await response.Content.ReadAsByteArrayAsync();
                var qrCodeAsBase64 = Convert.ToBase64String(qrCodeBytes);

                // Return the QR code as an image source in an HTML string
                return Content($"<img src='data:image/png;base64,{qrCodeAsBase64}' />", "text/html");
            }

            return Content("");
        }



        public IActionResult cancel(string sessionId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.PaymentID == sessionId);
            if (booking != null)
            {
                booking.Status = "Failed";
                _context.SaveChanges();
            }
            return View("Paymentfailed");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
