using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenViewModel
{
    public class OrderViewModel
    {
        public long OrderId { get; set; }
        public DateTime OrderDt { get; set; }
        public string Email { get; set; } //email of user who places the order

        //Shipping Details
        public string IsShipExistingAddress { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipArea { get; set; }
        public string ShipPinCode { get; set; }
        public string ShipPhone { get; set; }

        //Billing Details
        public string IsBillExistingAddress { get; set; }
        public string BillName { get; set; }
        public string BillAddress { get; set; }
        public string BillCity { get; set; }
        public string BillArea { get; set; }
        public string BillPinCode { get; set; }
        public string BillPhone { get; set; }

        public byte PayMode { get; set; } //1-cash, 2-Cheque, 3-Online
        public string PayRemark { get; set; }

        public int? CouponId { get; set; }//FK
        public float DeliveryCharges { get; set; }
        public float DiscountAmt { get; set; }
        public float TotalAmt { get; set; }
        public float TaxAmt { get; set; }
        public bool IsPaid { get; set; }

        public bool IsTermsAndCondition { get; set; }

        public string OrderStatus { get; set; }

        public List<OrderDetailsViewModel> ProductList { get; set; }

    }

    public class OrderDetailsViewModel
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public float Price { get; set; }
        public float Qty { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }

    }

    public class OrderListViewModel
    {
        public long OrderId { get; set; }
        public string OrderDt { get; set; }
        public string ShipName { get; set; }
        public float GrandTotal { get; set; }
        public bool PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public List<OrderDetailsViewModel> ProductList { get; set; }
    }

    public class OrderGridViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<OrderListViewModel> OrderList { get; set; }
    }

    public class AddressViewModel
    {
        public int UserId { get; set; }

        //Shipping Details
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Address is Blank.")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "City is Blank.")]
        public string ShipCity { get; set; }

        [Required(ErrorMessage = "Area is Blank.")]
        public string ShipArea { get; set; }

        [Required(ErrorMessage = "Pincode is Blank.")]
        public string ShipPinCode { get; set; }

        //Billing Details
        public string BillName { get; set; }

        [Required(ErrorMessage = "Address is Blank.")]
        public string BillAddress { get; set; }

        [Required(ErrorMessage = "City is Blank.")]
        public string BillCity { get; set; }

        [Required(ErrorMessage = "Area is Blank.")]
        public string BillArea { get; set; }

        [Required(ErrorMessage = "Pincode is Blank.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pincode.")]
        public string BillPinCode { get; set; }


    }

    public class OrderConfirmViewModel
    {
        public int UserId { get; set; }

        //Shipping Details
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Address is Blank.")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "City is Blank.")]
        public string ShipCity { get; set; }

        [Required(ErrorMessage = "Area is Blank.")]
        public string ShipArea { get; set; }

        [Required(ErrorMessage = "Pincode is Blank.")]
        public string ShipPinCode { get; set; }

        public string ShipPhone { get; set; }


        //Billing Details
        public string BillName { get; set; }

        [Required(ErrorMessage = "Address is Blank.")]
        public string BillAddress { get; set; }

        [Required(ErrorMessage = "City is Blank.")]
        public string BillCity { get; set; }

        [Required(ErrorMessage = "Area is Blank.")]
        public string BillArea { get; set; }

        [Required(ErrorMessage = "Pincode is Blank.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pincode.")]
        public string BillPinCode { get; set; }

        public string BillPhone { get; set; }

        public bool PaymentStatus { get; set; }

        public string PaymentId { get; set; }

        public DateTime PaymentDate { get; set; }

        public float TotalAmt { get; set; }

        public int? CouponId { get; set; }//FK

        public float DeliveryCharges { get; set; }

        public float DiscountAmt { get; set; }
    }

    public class PayOrderViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public float Amount { get; set; }
        public string Phone { get; set; }

    }

    public class CollectionViewModel //Collection by date
    {
        public DateTime OrderDt { get; set; }

        public float TotalAmount { get; set; }

        public float DeliveryCharges { get; set; }

        public float DiscountAmt { get; set; }

        public float FinalAmount { get; set; }

        public byte PayMode { get; set; }

        public string PayRemark { get; set; }

    }

    public class SaleProductViewModel //SaleProduct by date
    {
        public string Name { get; set; }

        public double Qty { get; set; }

        public DateTime OrderDt { get; set; }
    }

    public class BillDetailViewModel
    {
        public long OrderId { get; set; }
        public DateTime OrderDt { get; set; }

        public float DeliveryCharges { get; set; }
        public float DiscountAmt { get; set; }
        public float TotalAmt { get; set; }
        public int ProductId { get; set; }
        public float Price { get; set; }
        public float Qty { get; set; }
        public string Name { get; set; }
        public string CutType { get; set; }
    }
}
