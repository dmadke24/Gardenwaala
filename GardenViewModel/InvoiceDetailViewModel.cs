using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{
    public class InvoiceDetailViewModel
    {
        [Required(ErrorMessage="Particulars are Blank")]
        [MaxLength(200,ErrorMessage="Particulars are too long")]
        public string Particulars { get; set; }

        public float Price { get; set; }

        public int Qty { get; set; }

        public long InvoiceDetailId { get; set; }

        //FK
        public long InvoiceId { get; set; }
    }
}
