using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiceApp.Models.DataModel
{
    public class AppUser : IdentityUser
    {
        public string? LoginStatus { get; set; }
        public int UserStatus { get; set; }
       
        public DateTime PwdExpiry { get; set; }
        public string Address { get; set; }


       [NotMapped]
        public string? Role { get; set; }
        [NotMapped]
        public string? RoleId { get; set; }
        //[NotMapped]
        //public IEnumerable<SelectListItem>? RoleList { get; set; }


    }
}
