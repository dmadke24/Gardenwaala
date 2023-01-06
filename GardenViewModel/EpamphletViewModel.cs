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
    public class EpamphletViewModel
    {
        public int EpamphletId { get; set; }
        public string Image { get; set; }    //this will contain image name

        [Required(ErrorMessage = "Subject is Blank")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is Blank")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Link is Blank")]
        [Url(ErrorMessage = "Invalid Link")]
        public string Link { get; set; }

        [FileSize(ErrorMessage = "You can upload file upto 2MB")]
        [CustomValidation.FileExtensions(ErrorMessage = "Invalid file")]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        public DateTime CreateDate { get; set; }

        public string DisplayCDate { get; set; }
    }


    public class EpamphletGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<EpamphletViewModel> EpamphletList { get; set; }
    }
}
