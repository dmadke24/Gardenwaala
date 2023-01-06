using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace GardenViewModel
{
    public class EnquiryViewModel
    {
        public int EnquiryId { get; set; }

        public DateTime EnqDt { get; set; }

        public string EnqDisplayDt { get; set; }

        [Required(ErrorMessage = " Name is Blank")]
        [MaxLength(60, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        // [Required(ErrorMessage = "Email-ID is Blank")]
        [EmailAddress(ErrorMessage = "Invalid Email-ID")]
        [MaxLength(150, ErrorMessage = "Email-ID is too long")]
        [Display(Name = "Email")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Contact No. is Blank")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Contact No.")]
        [Display(Name = "Contact No.")]
        public string AlternatContactNo { get; set; }

        [Required(ErrorMessage = "Description is Blank")]
        public string Description { get; set; }

        [Required(ErrorMessage="Subject is Blank")]
        [MaxLength(100, ErrorMessage = "Subject is too long")]
        public string Subject { get; set; }

    }
}
