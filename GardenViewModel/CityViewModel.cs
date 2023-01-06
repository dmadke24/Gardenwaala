using System;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class CityViewModel
    {
        public int CityId { get; set; }

        [Required(ErrorMessage="Select State")]
        public int StateId { get; set; } 

        [Required(ErrorMessage="City Name is Blank")]
        [StringLength(40,ErrorMessage="City Name is too long")]
        public string CityName { get; set; }

        public string SName { get; set; } //State Name

        public bool Status { get; set; }
    }
}
