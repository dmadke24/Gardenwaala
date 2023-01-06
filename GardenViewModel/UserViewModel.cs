using CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GardenViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Name is Blank")]
        [StringLength(50, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Display(Name = "Email-ID")]
        [Required(ErrorMessage = "Email-ID is Blank")]
        [EmailAddress(ErrorMessage = "Invalid Email-ID")]
        [StringLength(150, ErrorMessage = "Email-ID is too long")]
        // [System.Web.Mvc.Remote("ChkDuplicate","Account",ErrorMessage="Email-Id already in use")]
        public string Email { get; set; }

        [Display(Name = "Contact No")]
        [Required(ErrorMessage = "Contact No. is Blank")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
        public string ContactNo { get; set; }

        public string Address { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDt { get; set; }

        public string Gender { get; set; }

        public string Image { get; set; }

    }

    public class CurrentUserViewModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string ContactNo { get; set; }
    }

    public class UpdateUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Name is Blank")]
        [StringLength(150, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Display(Name = "Contact No")]
        [Required(ErrorMessage = "Contact No. is Blank")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
        public string ContactNo { get; set; }

        public string Image { get; set; } //this will contain file name

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is Blank")]
        public string Address { get; set; }


        public string Gender { get; set; }

        public string Email { get; set; }

        public string BillAddress { get; set; }

    }

    //public class UpdateUserWebApiModel
    //{
    //    public int UserId { get; set; }

    //    [Required(ErrorMessage = "Name is Blank")]
    //    [StringLength(150, ErrorMessage = "Name is too long")]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "Contact No. is Blank")]
    //    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
    //    public string ContactNo { get; set; }

    //    [Required(ErrorMessage = "Address is Blank")]
    //    public string Address { get; set; }
    //    public string Gender { get; set; }

    //    public string Email { get; set; }

    //    public string BillAddress { get; set; }
    //}

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is Blank")]
        [StringLength(150, ErrorMessage = "Name is too long")]
        [Display(Name = "Contact Person")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email-ID is Blank")]
        [EmailAddress(ErrorMessage = "Invalid Email-ID")]
        [StringLength(150, ErrorMessage = "Email-ID is too long")]
        // [System.Web.Mvc.Remote("ChkDuplicate", "Account", "UserAdmin", ErrorMessage = "Email-Id already in use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No. is Blank")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
        public string ContactNo { get; set; }

        [StringLength(10, MinimumLength = 8, ErrorMessage = "Password should 8 To 10 Character long")]
        [Required(ErrorMessage = "Password is Blank")]
        [Display(Name = "Create Password")]
        public string Password { get; set; }

        public DateTime CreatedDt { get; set; }
    }

    //public class RegisterWebApiViewModel
    //{
    //    [Required(ErrorMessage = "Name is Blank")]
    //    [StringLength(150, ErrorMessage = "Name is too long")]
    //    [Display(Name = "Contact Person")]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "Email-ID is Blank")]
    //    [EmailAddress(ErrorMessage = "Invalid Email-ID")]
    //    [StringLength(150, ErrorMessage = "Email-ID is too long")]
    //    // [System.Web.Mvc.Remote("ChkDuplicate", "Account", "UserAdmin", ErrorMessage = "Email-Id already in use")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "Contact No. is Blank")]
    //    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
    //    public string ContactNo { get; set; }

    //    [StringLength(10, MinimumLength = 8, ErrorMessage = "Password should 8 To 10 Character long")]
    //    [Required(ErrorMessage = "Password is Blank")]
    //    public string Password { get; set; }

    //    public DateTime CreatedDt { get; set; }
    //}

    public class OnlyEmailViewModel
    {
        [Required(ErrorMessage = "Email-ID is Blank")]
        [EmailAddress(ErrorMessage = "Invalid Email-ID")]
        [StringLength(150, ErrorMessage = "Email-ID is too long")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is Blank")]
        [EmailAddress(ErrorMessage = "Invalid Email-ID")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is Blank")]
        public string Password { get; set; }

    }

    public class VisitorCountViewModel
    {
        public int VisitorYear { get; set; }
        public int VisitorMonth { get; set; }
        public int VisitorCnt { get; set; }
        public string Mon { get; set; }
    }

    public class UserReportViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string ContactNo { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

    }
}
