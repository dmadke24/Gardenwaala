using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GardenViewModel
{
    public class CareerViewModel
    {

        public int JobTitleId { get; set; }

        [Required(ErrorMessage = "Position is Blank")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Experience is Blank")]
        public string Experience { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Requirements are Blank")]
        public string Requirements { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Location is Blank")]
        public string Location { get; set; }

        public string City { get; set; }
        public DateTime CreatedDt { get; set; }


        [Required(ErrorMessage = "Qualification is Blank")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Department is Blank")]
        public string Department { get; set; }

        public string Package { get; set; }

        [Required(ErrorMessage = "keyskills are Blank")]
        [Display(Name = "Key Skills")]
        public string keyskills { get; set; }

        [Display(Name = "No. of Openings")]
        public int Openings { get; set; }

        public bool Status { get; set; }
    }
}
