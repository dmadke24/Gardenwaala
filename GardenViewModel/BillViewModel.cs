using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{
    public class BillViewModel
    {
        public long BillNo { get; set; }
        public long InvoiceId { get; set; }
        public float TaxPercent { get; set; }
        public IEnumerable<InvoiceDetailViewModel> InvoiceDetail { get; set; }
    }
}
