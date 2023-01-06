using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GardenViewModel
{
    public class TaxViewModel
    {
        [Required(ErrorMessage = "CGST is Blank")]
        public float CGST { get; set; }

        [Required(ErrorMessage = "SGST is Blank")]
        public float SGST { get; set; }

        [Display(Name = "Service Tax")]
        public float? ServiceTax { get; set; }

        public float? VAT { get; set; }
    }
}
