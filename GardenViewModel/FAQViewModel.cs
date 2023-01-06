using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{
    public class FAQViewModel
    {
        public int FaqId { get; set; }

        [Required(ErrorMessage = "Question is Blank")]
        [MaxLength(100, ErrorMessage = "Question is too long")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Answer is Blank")]
        public string Answer { get; set; }

        public bool Status { get; set; }
    }

    public class FAQGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<FAQViewModel> FAQList { get; set; }

    }
}
