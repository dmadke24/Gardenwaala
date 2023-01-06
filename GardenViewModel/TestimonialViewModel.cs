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
    public class TestimonialViewModel
    {
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "Client Name is Blank")]
        [MaxLength(50, ErrorMessage = "Client Name is too long")]
        [Display(Name = "Client Name")]
        public string CName { get; set; } //Client Name

        public string Image { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [MaxLength(50, ErrorMessage = "Profession Name is too long")]
        public string Profession { get; set; }

        [MaxLength(50, ErrorMessage = "Organization Name is too long")]
        public string Organization { get; set; }

        public string Description { get; set; }
        public bool Status { get; set; }

        public string OldFile { get; set; }// sending the old file in edit

    }

}
