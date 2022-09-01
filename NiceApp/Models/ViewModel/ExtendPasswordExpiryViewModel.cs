using System.ComponentModel.DataAnnotations;

namespace NiceApp.Models.ViewModel
{
    public class ExtendPasswordExpiryViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Please confirm the correct password.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
