using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using CustomValidation;
namespace GardenViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is Blank")]
        [MaxLength(100, ErrorMessage = "Product Name is too long")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Blank")]
        public float Price { get; set; }

        [Display(Name = "Offer Price")]
        public float? OfferPrice { get; set; }


        [Required(ErrorMessage = "Category is Blank")]
        [Display(Name = "Select Category")]
        public int CategoryId { get; set; } //FK

        [Required(ErrorMessage = "Sub Category is Blank")]
        [Display(Name = "Select Sub Category")]
        public int SubCategoryId { get; set; } //FK

        public string Image1 { get; set; }
        public string OldImage1 { get; set; }

        [FileSize(1, ErrorMessage = "You can upload file upto 1MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(1)")]
        public HttpPostedFileBase ImgFile1 { get; set; }

        public string Image2 { get; set; }
        public string OldImage2 { get; set; }

        [FileSize(1, ErrorMessage = "You can upload file upto 1MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(2)")]
        public HttpPostedFileBase ImgFile2 { get; set; }

        public string Image3 { get; set; }
        public string OldImage3 { get; set; }

        [FileSize(1, ErrorMessage = "You can upload file upto 1MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(3)")]
        public HttpPostedFileBase ImgFile3 { get; set; }

        public bool Status { get; set; }

        public bool HomeSlide { get; set; }

        public bool BestSeller { get; set; }

        [Range(0, 255, ErrorMessage = "Invalid Number, Enter between 0-255")]
        public byte? Priority { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        [Required(ErrorMessage = "No Of Pieces is Blank")]
        [Display(Name = "No Of Pieces")]
        public string NoOfPieces { get; set; }

        [Display(Name = "Net Weight")]
        public string NetWeight { get; set; }

        [Display(Name = "Gross Weight")]
        public string GrossWeight { get; set; }


        [Display(Name = "Height")]
        public string Height { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [RegularExpression(@"^\d{0,5}$", ErrorMessage = "Invalid Stock.")]
        public int Stock { get; set; }
    }

    //food menu for order detail with cut types
    public class ProductOrderViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public float Price { get; set; }

        public int CategoryId { get; set; } //FK

        public int SubCategoryId { get; set; } //FK

        public string Image1 { get; set; }
        public string OldImage1 { get; set; }

        public HttpPostedFileBase ImgFile1 { get; set; }

        public string Image2 { get; set; }
        public string OldImage2 { get; set; }

        public HttpPostedFileBase ImgFile2 { get; set; }

        public bool Status { get; set; }

        public bool HomeSlide { get; set; }

        public bool BestSeller { get; set; }

        public byte? Priority { get; set; }

        public string TypeName { get; set; }

        public string NoOfPieces { get; set; }

        public string NetWeight { get; set; }

        public string GrossWeight { get; set; }

        public string Height { get; set; }

        public string Size { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

    }

}
