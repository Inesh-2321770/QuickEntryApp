using System.ComponentModel.DataAnnotations;

namespace QuickEntryApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        public bool isAdmin = false;
    }
}
