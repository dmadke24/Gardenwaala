using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Country Name is Blank")]
        [StringLength(15, ErrorMessage = "Country Name is too long")]
        public string CName { get; set; }
    }
}
