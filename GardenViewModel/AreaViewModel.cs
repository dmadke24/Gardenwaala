using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class AreaViewModel
    {
        public int AreaId { get; set; }

        [Display(Name = "Area")]
        [Required(ErrorMessage = "Area Name is Blank")]
        [StringLength(100, ErrorMessage = "Area Name is too long")]
        public string AName { get; set; }

        [Required(ErrorMessage = "Select City")]
        public int CityId { get; set; }

        public bool Status { get; set; }

        public string CityName { get; set; }

        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "Invalid Pincode")]
        public string Pincode { get; set; }


    }
}
