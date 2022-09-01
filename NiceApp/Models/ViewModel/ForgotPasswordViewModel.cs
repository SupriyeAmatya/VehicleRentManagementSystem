using System.ComponentModel.DataAnnotations;

namespace NiceApp.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
