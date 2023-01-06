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
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage="Type is Blank")]
        [Display(Name="Type")]

        [StringLength(100,ErrorMessage="Type is too long")]
        public string CategoryName { get; set; }

        public string Image { get; set; }    //this will contain image name

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }    //this will be the actual image file

        public bool Status { get; set; }

        public string OldFile { get; set; }// sending the old file in edit

    }

    public class TypeAPIViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }    //this will contain image name

    }
    
}
