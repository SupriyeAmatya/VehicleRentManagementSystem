using System.ComponentModel.DataAnnotations;

namespace NiceApp.Models.ViewModel
{
    public class PasswordExpiryViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
