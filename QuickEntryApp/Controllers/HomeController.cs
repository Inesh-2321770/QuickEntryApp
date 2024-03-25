using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickEntryApp.Models;
using System.Diagnostics;

namespace QuickEntryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet("api/privacy")]
        public IActionResult Privacy()
        {
            // Your code logic here
            return Ok("Hello boii");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("isAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            // Check if the JWT cookie exists
            if (Request.Cookies.ContainsKey("jwt"))
            {
                // If the JWT cookie exists, return true
                return Ok(new { isAuthenticated = true });
            }
            else
            {
                // If the JWT cookie does not exist, return false
                return Ok(new { isAuthenticated = false });
            }
        }



        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the JWT cookie
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "Logged out" });
        }

    }
}
