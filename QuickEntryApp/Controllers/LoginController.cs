using Microsoft.AspNetCore.Mvc;
using QuickEntryApp.Models;
using QuickEntryApp.Data;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace QuickEntryApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly QuickEntryAppDbContext _context;

        public LoginController(QuickEntryAppDbContext context)
        {
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Verify the password with bcrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            try
            {
                // Call the GenerateJwt API to get the JWT token
                var client = new HttpClient();
                var content = new StringContent("\"" + user.Username + "\"", Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44354/generate-jwt", content);
                var tokenResponse = await response.Content.ReadAsStringAsync();
                var jwtToken = JsonConvert.DeserializeObject<dynamic>(tokenResponse).token;
                string encodedToken = System.Net.WebUtility.UrlEncode(jwtToken.ToString());

                Response.Cookies.Append("jwt", encodedToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax, // or SameSiteMode.Strict
                });

                if (user.IsAdmin)
                {
                    return RedirectToAction("AdminDashboard", "AdminDashboard");
                }
                else
                {
                    return RedirectToAction("BookingForm", "BookingForm");
                }
            }
            catch (Exception ex)
            {
               
                return RedirectToAction("Register", "Register");
            }

        }



    }
}
