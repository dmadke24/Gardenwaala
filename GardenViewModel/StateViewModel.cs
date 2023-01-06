using System;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class StateViewModel
    {
        public int StateId { get; set; }

        [Required(ErrorMessage = "State Name is blank")]
        [StringLength(15, ErrorMessage = "State Name is too long")]
        public string SName { get; set; }

        [Required(ErrorMessage = "Select Country")]
        public int CountryId { get; set; }

        public string CName { get; set; } //Country Name
    }
}
