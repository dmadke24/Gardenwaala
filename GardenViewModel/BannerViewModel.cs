using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using CustomValidation;


namespace GardenViewModel
{
    public class BannerViewModel
    {
        public int BannerId { get; set; }
        public string Image { get; set; }    //this will contain image name

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name="Image")]
        public HttpPostedFileBase ImageFile { get; set; }    //this will be the actual image file

        [Required(ErrorMessage="Title is Blank")]
        public string Title { get; set; }
        public bool Status { get; set; }

        public string OldFile { get; set; }// sending the old file in edit
    }

}
