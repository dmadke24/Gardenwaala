using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GardenViewModel
{
    public class TempCartViewModel
    {
        public int TempId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string ProdName { get; set; }

        public string Image { get; set; }


    }

    public class AddToTempCartViewModel
    {
        public int prodid { get; set; }

        public int qty { get; set; }

        public float price { get; set; }

        public int UserId { get; set; }

    }

    public class DeliveryDetViewModel
    {
        public string OrderId { get; set; }
        public float DeliveryCrg
        {
            get
            {
                return 35.00F;
            }
        }
        public AddressViewModel DeliveryAddress { get; set; }

        public IEnumerable<TempCartViewModel> TempCart { get; set; }

        public double SubTotal
        {
            get
            {
                return TempCart.Sum(t => t.Price * t.Quantity);
            }

        }

        public double GrandTotal
        {
            get
            {
                return SubTotal;
            }
        }


    }

}
