using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; } // in our project email is username

        [Required(ErrorMessage="Old Password is Blank")]
        [Display(Name="Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is Blank")]
        [Display(Name = "New Password")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Password should 8 To 10 Character long")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Blank")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword",ErrorMessage="New Password & Confirm Password do not Match")]
        public string ConfirmPassword { get; set; }
    }





}
