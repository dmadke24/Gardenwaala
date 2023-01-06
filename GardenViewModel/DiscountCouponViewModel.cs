using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CustomValidation;
using System.Web;
namespace GardenViewModel
{
    public class DiscountCouponViewModel
    {
        public int CouponId { get; set; }

        [Required(ErrorMessage = "Select Food Type")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Code is Blank")]
        [StringLength(50, ErrorMessage = "Code is too long")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Title is Balnk")]
        [StringLength(100, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Valid Date From is Blank")]
        [Display(Name = "Valid From")]
        public string ValidFrom { get; set; }

        [Required(ErrorMessage = "Valid Date to is Blank")]
        [Display(Name = "Valid To")]
        [IsGreater(OtherDate = "ValidFrom", ErrorMessage = "Invalid Date, Should be Greater than From Date")]
        public string ValidTo { get; set; }

        [Required(ErrorMessage = "Discount Percet is Blank")]
        [Display(Name = "Discount Percent")]
        public float DiscountPercent { get; set; }

        public string Image { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }    //this will be the actual image file

        public string OldFile { get; set; }// sending the old file in edit
    }

    public class DiscountCouponPercetgViewModel
    {
        public float DiscPercent { get; set; }
    }

}
