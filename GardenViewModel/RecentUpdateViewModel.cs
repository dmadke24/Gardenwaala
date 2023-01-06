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
    public class RecentUpdateViewModel
    {
        public int UpdateId { get; set; }

        [Required(ErrorMessage = "Title is Blank")]
        [StringLength(100, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        public string UpdateDt { get; set; }

        [Required(ErrorMessage = "Description is Blank")]
        public string Description { get; set; }

        public string Image { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(1)")]
        public HttpPostedFileBase ImageFile { get; set; }

        public string Image1 { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(2)")]
        public HttpPostedFileBase ImageFile1 { get; set; }

        public string Image2 { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image(3)")]
        public HttpPostedFileBase ImageFile2 { get; set; }

        [Url(ErrorMessage = "Invalid Link")]
        [StringLength(200, ErrorMessage = "URL is too long")]
        public string Link { get; set; }

        public string ScheduleDt { get; set; }

        public bool Status { get; set; }

        public string OldFile { get; set; }
        public string OldFile1 { get; set; }
        public string OldFile2 { get; set; }

    }

    public class RecentUpdateGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<RecentUpdateViewModel> RecentUpdateList { get; set; }
    }
}
