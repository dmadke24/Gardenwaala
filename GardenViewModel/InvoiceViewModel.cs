using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{
    public class InvoiceViewModel
    {
        public long InvoiceId { get; set; }

        public DateTime InvoiveDt { get; set; }

        public float Amount { get; set; }

        public float TaxAmount { get; set; }

        public float DiscountAmount { get; set; }

        public bool IsPaid { get; set; }

        public PaymentMode PayMode { get; set; }

        //FK
        public long CustomerId { get; set; }
        public long BookingId { get; set; }
        public int? CouponId { get; set; }
    }

    public enum PaymentMode : byte
    {
        Cash = 1,
        Card,
        Other
    }
}
