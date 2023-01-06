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
    public class SubCategoryViewModel
    {
        public int SubCategoryId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } //Category Name

        [Required(ErrorMessage = "Sub Category is Blank")]
        [Display(Name = "Sub Category")]
        [StringLength(100, ErrorMessage = "Sub Category is too long")]
        public string SubCategoryName { get; set; }

        public string Image { get; set; }    //this will contain image name

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }    //this will be the actual image file

        public string OldFile { get; set; }// sending the old file in edit


    }

    public class SubCategoriesAPIViewModel
    {
        public int SubCategoryId { get; set; }

        public string Image { get; set; }    //this will contain image name

        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; }
    }

}
