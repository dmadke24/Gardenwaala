using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GardenViewModel;
using Gardenwaala.Areas.Client.Models;

namespace BAL
{
    public interface IOrderRepository
    {
        //bool Add(DeliveryDetViewModel model, string CouponCode, float DeliveryCrg, int uid);
        bool Add(OrderViewModel model, int uid, IEnumerable<TempCartViewModel> tempCart, string CouponCode = "", float DeliveryCrg = 0);
        OrderViewModel GetById(long id);
        OrderGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);
        OrderGridViewModel GetAllStatusOrder(string sort, string sortDir, int skipRecords, int pgSize, string searchStr);

        bool Delete(long id);
        IEnumerable<OrderDetailsViewModel> GetOrderDetail(int OrderId);
        void ChangeStatus(IEnumerable<long> Ids, bool status);
        IEnumerable<OrderListViewModel> GetUserOrder(int userid);
        PayOrderViewModel GetOrderPaymentDetail(DeliveryDetViewModel model, string CouponCode, float DeliveryCrg, int uid, string UserId);
        OrderViewModel GetOrderDetailById(long oid, int uid);
        int OrderCnt();
        CountViewModel TodaysOrderCount();
        bool OrderConfirmation(OrderConfirmViewModel model);
        void ChangeOrderStatus(IEnumerable<long> Ids, string status);
        IEnumerable<BillDetailViewModel> GenerateBill(long oid);

    }
    public class OrderRepository : IOrderRepository
    {
        MyDBContext db = new MyDBContext();

        public OrderViewModel GetById(long id)
        {
            OrderViewModel model = null;
            Order obj = db.Orders.Find(id);
            if (obj != null)
            {
                model = new OrderViewModel();
                model.BillAddress = obj.BillAddress;
                model.BillArea = obj.BillArea;
                model.BillCity = obj.BillCity;
                model.BillName = obj.BillName;
                model.BillPhone = obj.BillPhone;
                model.BillPinCode = obj.BillPinCode;
                model.CouponId = obj.CouponId;
                model.DeliveryCharges = obj.DeliveryCharges;
                model.DiscountAmt = obj.DiscountAmt;
                model.Email = obj.Email;
                model.IsPaid = obj.PaymentStatus;
                model.OrderDt = obj.OrderDt;
                model.OrderId = obj.OrderId;
                model.PayMode = obj.PayMode;
                model.PayRemark = obj.PayRemark;
                model.ShipAddress = obj.ShipAddress;
                model.ShipArea = obj.ShipArea;
                model.ShipCity = obj.ShipCity;
                model.ShipName = obj.ShipName;
                model.ShipPhone = obj.ShipPhone;
                model.ShipPinCode = obj.ShipPinCode;
                model.TaxAmt = obj.TaxAmt;
                model.TotalAmt = obj.TotalAmt;

                List<OrderDetailsViewModel> lstDetails = new List<OrderDetailsViewModel>();
                foreach (OrderDetail detObj in obj.OrderDetails)
                {
                    OrderDetailsViewModel detModel = new OrderDetailsViewModel();
                    detModel.ProductName = detObj.Product.Name;
                    detModel.Price = detObj.Price;
                    detModel.Qty = detObj.Qty;
                    lstDetails.Add(detModel);
                }
                model.ProductList = lstDetails;
            }
            return model;
        }

        public OrderGridViewModel GetAll(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            OrderGridViewModel gridModel = new OrderGridViewModel();

            IQueryable<Order> lstOrder = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstOrder = db.Orders;
            }
            else
            {
                lstOrder = db.Orders.Where(o => o.ShipName.ToLower().Contains(searchStr));
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "shipname":
                    lstOrder = sortDir == "asc" ? lstOrder.OrderBy(o => o.ShipName) : lstOrder.OrderByDescending(o => o.ShipName);
                    break;
                case "orderdt":
                    lstOrder = sortDir == "asc" ? lstOrder.OrderBy(o => o.OrderDt) : lstOrder.OrderByDescending(o => o.OrderDt);
                    break;
                default:
                    lstOrder = lstOrder.OrderBy(o => o.OrderId).OrderByDescending(o => o.OrderId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstOrder.Count();

            //pagination code start******************************************************
            lstOrder = lstOrder.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<OrderListViewModel> lstModel = new List<OrderListViewModel>();

            foreach (Order obj in lstOrder.ToList<Order>())
            {
                OrderListViewModel model = new OrderListViewModel();

                model.OrderId = obj.OrderId;
                model.OrderDt = obj.OrderDt.ToString("dd-MMM-yyyy");
                model.ShipName = obj.ShipName;
                model.GrandTotal = obj.TotalAmt;
                model.PaymentStatus = obj.PaymentStatus;
                List<OrderDetailsViewModel> lstDetails = new List<OrderDetailsViewModel>();
                foreach (OrderDetail detObj in obj.OrderDetails)
                {
                    OrderDetailsViewModel detModel = new OrderDetailsViewModel();
                    detModel.ProductName = detObj.Product.Name;
                    detModel.Price = detObj.Price;
                    detModel.Qty = detObj.Qty;
                    lstDetails.Add(detModel);
                }
                model.ProductList = lstDetails;

                lstModel.Add(model);
            }

            gridModel.OrderList = lstModel;
            return gridModel;
        }

        public bool Delete(long id)
        {
            try
            {
                Order obj = db.Orders.Find(id);
                if (obj != null)
                {
                    db.Orders.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<OrderDetailsViewModel> GetOrderDetail(int OrderId)
        {
            IEnumerable<OrderDetailsViewModel> lstOrderDetail = db.Database.SqlQuery<OrderDetailsViewModel>("Select * From OrderDetails where OrderId=" + OrderId);
            return lstOrderDetail;
        }

        private long NewId()
        {
            long oldid = db.Orders.Select(a => a.OrderId).DefaultIfEmpty().Max();
            long myid, newid;
            if (oldid == 0)
            {
                myid = 1;
                return myid;
            }
            else
            {
                newid = oldid + 1;
                return (newid);
            }
        }

        private long NewOrderDetId()
        {
            long oldid = db.OrderDetails.Select(a => a.OrderDetId).DefaultIfEmpty().Max();
            long myid, newid;
            if (oldid == 0)
            {
                myid = 1;
                return myid;
            }
            else
            {
                newid = oldid + 1;
                return (newid);
            }
        }

        //public bool Add(DeliveryDetViewModel model, string CouponCode, float DeliveryCrg, int uid)
        //{
        //    try
        //    {
        //        //Get User details
        //        User uObj = db.Users.Where(u => u.UserId == uid).FirstOrDefault();

        //        //Coupon details
        //        DiscountCoupon cObj = db.DiscountCoupons.Where(c => c.Code == CouponCode).FirstOrDefault();

        //        double DiscountAmt = (model.SubTotal * cObj.DiscountPercent) / 100;
        //        double TotalAmt = model.GrandTotal - DiscountAmt + DeliveryCrg;

        //        Order obj = new Order();
        //        OrderDetail objOdet = new OrderDetail();

        //        obj.OrderId = NewId(); //Id generated manually
        //        obj.BillAddress = model.DeliveryAddress.BillAddress;
        //        obj.BillArea = model.DeliveryAddress.BillArea;
        //        obj.BillCity = model.DeliveryAddress.BillCity;
        //        obj.BillName = uObj.Name;
        //        obj.BillPhone = uObj.ContactNo;
        //        obj.BillPinCode = model.DeliveryAddress.BillPinCode;
        //        obj.CouponId = cObj.CouponId;
        //        obj.DeliveryCharges = DeliveryCrg;
        //        obj.DiscountAmt = Convert.ToSingle(DiscountAmt);
        //        obj.Email = uObj.Email;
        //        obj.PaymentStatus = true;
        //        obj.OrderDt = DateTime.Now;
        //        obj.PayMode = 1;
        //        obj.PayRemark = "Paid";
        //        obj.ShipAddress = model.DeliveryAddress.ShipAddress;
        //        obj.ShipArea = model.DeliveryAddress.ShipArea;
        //        obj.ShipCity = model.DeliveryAddress.ShipCity;
        //        obj.ShipName = uObj.Name;
        //        obj.ShipPhone = uObj.ContactNo;
        //        obj.ShipPinCode = model.DeliveryAddress.ShipPinCode;
        //        obj.User_UserId = uObj.UserId;


        //        obj.TaxAmt = 0;
        //        obj.TotalAmt = Convert.ToSingle(TotalAmt);
        //        db.Orders.Add(obj);

        //        long NeworderDetailId = NewOrderDetId();

        //        foreach (TempCartViewModel tModel in model.TempCart)
        //        {
        //            objOdet = new OrderDetail();
        //            objOdet.OrderDetId = NeworderDetailId;
        //            objOdet.OrderId = obj.OrderId;
        //            objOdet.Price = Convert.ToSingle(tModel.Price);
        //            objOdet.Qty = tModel.Quantity;
        //            objOdet.ProductId = tModel.ProductId;
        //            db.OrderDetails.Add(objOdet);
        //            NeworderDetailId++;
        //        }

        //        //Remove Temperory Cart
        //        IEnumerable<TempCart> tObj = db.TempCarts.Where(t => t.UserId == uid);
        //        if (tObj != null)
        //        {
        //            foreach (TempCart tc in tObj)
        //            {
        //                db.TempCarts.Remove(tc);
        //            }
        //        }

        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool Add(OrderViewModel model, int uid, IEnumerable<TempCartViewModel> tempCart, string CouponCode = "", float DeliveryCrg = 0)
        {
            try
            {
                //Get User details
                User uObj = db.Users.Where(u => u.UserId == uid).FirstOrDefault();
                double TotalAmt = 0;
                //Coupon details
                if (string.IsNullOrWhiteSpace(CouponCode) == false)
                {
                    DiscountCoupon cObj = db.DiscountCoupons.Where(c => c.Code == CouponCode).FirstOrDefault();

                    double DiscountAmt = 0;//(model.SubTotal * cObj.DiscountPercent) / 100;
                    // model.GrandTotal - DiscountAmt + DeliveryCrg;
                }

                Order obj = new Order();
                OrderDetail objOdet = new OrderDetail();

                obj.OrderId = NewId(); //Id generated manually
                if (model.IsBillExistingAddress == "N")
                {
                    obj.BillAddress = model.BillAddress;
                    obj.BillArea = model.BillArea;
                    obj.BillCity = "Pune";
                    obj.BillName = model.BillName;
                    obj.BillPhone = model.BillPhone;
                    obj.BillPinCode = model.BillPinCode;
                }
                else //it is an existing address, so transfer the details of the user
                {
                    obj.BillAddress = uObj.Address;
                    obj.BillArea = string.Empty;
                    obj.BillCity = "Pune";
                    obj.BillName = uObj.Name;
                    obj.BillPhone = uObj.ContactNo;
                    obj.BillPinCode = "411014";
                }
                //obj.CouponId = cObj.CouponId;
                obj.DeliveryCharges = 0;
                //obj.DiscountAmt = Convert.ToSingle(DiscountAmt);
                obj.Email = uObj.Email;
                obj.PaymentStatus = true;
                obj.OrderDt = DateTime.Now;
                obj.PayMode = 1;
                obj.PayRemark = model.PayRemark;

                if (model.IsShipExistingAddress == "N")
                {
                    obj.ShipAddress = model.ShipAddress;
                    obj.ShipArea = model.ShipArea;
                    obj.ShipCity = model.ShipCity;
                    obj.ShipName = model.ShipName;
                    obj.ShipPhone = model.ShipPhone;
                    obj.ShipPinCode = model.ShipPinCode;
                }
                else
                {
                    obj.ShipAddress = uObj.Address;
                    obj.ShipArea = string.Empty;
                    obj.ShipCity = "Pune";
                    obj.ShipName = uObj.Name;
                    obj.ShipPhone = uObj.ContactNo;
                    obj.ShipPinCode = "411014";
                }
                obj.User_UserId = uObj.UserId;

                obj.TaxAmt = 0;


                long NeworderDetailId = NewOrderDetId();
                string orderList = "";
                foreach (TempCartViewModel cartItem in tempCart)
                {
                    objOdet = new OrderDetail();
                    objOdet.OrderDetId = NeworderDetailId;
                    objOdet.OrderId = obj.OrderId;
                    objOdet.Price = Convert.ToSingle(cartItem.Price);
                    objOdet.Qty = cartItem.Quantity;
                    objOdet.ProductId = cartItem.ProductId;
                    TotalAmt += cartItem.Price * cartItem.Quantity;
                    db.OrderDetails.Add(objOdet);
                    NeworderDetailId++;
                    //get productname from product id
                    string ProductName = db.Products.Where(p => p.ProductId == cartItem.ProductId).Select(p => p.Name).FirstOrDefault();
                    orderList += "<tr>" +
                        "<td width = '30%' style = 'padding-top:4%;padding- bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold;float:left'><img src='https://gardenwaala.com/Areas/Admin/UploadImg/ProductImg/" + cartItem.Image + "' title='" + ProductName + "' alt='" + ProductName + "' style='width:70px'></td>" +
                                            "<td width = '50%' style = 'padding-top:4%;padding- bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold;float:left'>" + ProductName + "<br><span style = 'font-size:12px;color:#828282;font-family:'Roboto',sans-serif'> (" + cartItem.Quantity + " x ₹" + cartItem.Price.ToString("0.00") + ")</span></td>" +
                        "<td width = '20%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:right;font-weight:bold;float:right'>₹" + cartItem.Price * cartItem.Quantity + ".00</td>" + "</tr>";
                }

                obj.TotalAmt = Convert.ToSingle(TotalAmt);
                db.Orders.Add(obj);

                db.SaveChanges();

                //EMAIL to CLIENT
                string emailMsg = "<tr>" +
                    "<tr><td>&nbsp;</td></tr><td style='float:right'> Order ID: " + obj.OrderId + " </td></tr>" +
                        "<table border='0' width='92%' cellpadding='0' align='center' cellspacing='0'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td style = 'border-collapse:collapse'>" +
                                        "<table border = '0' width = '100%' cellpadding = '0' align = 'center' cellspacing = '0' style = 'margin-top:5px;border-spacing:0;border-collapse:collapse'>" +
                                        "<table width='100%'>" +
                                            "<tbody>" +
                                                orderList +
                                            "</tbody>" +
                                        "</table>" +
                                        "<table border = '0' width = '100%' cellpadding = '0' cellspacing = '0' style = 'margin-top:20px;border-spacing:0;border-collapse:collapse'>" +
                                        "<tbody>" +
                                        "<tr>" +
                                        "<td width = '30%' style = 'padding-top:4%;padding- bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold'>&nbsp;&nbsp;&nbsp;" +
                                            "</td>" +
                                            "<td width = '50%' style = 'padding-top:4%;padding- bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold'>" +
                    "Delivery Charge" +
                                            "</td>" +
                                            "<td width = '20%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:right;font-weight:bold'>₹0.00" +
                                            "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "<td width = '30%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold'>&nbsp;&nbsp;&nbsp;" +
                                            "</td>" +
                                            "<td width = '50%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold'>Taxes" +
                                            "</td>" +
                                            "<td width = '20%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:right;font-weight:bold'>₹0.00" +
                                            "</td>" +
                                    "</tr>" +
                                "</tbody>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style = 'border-collapse:collapse'>" +
                            "<table border = '0' width = '100%' cellpadding = '0' cellspacing = '0' style = 'border-spacing:0;border-collapse:collapse;width: 650px !inportant'>" +
                                "<tbody>" +
                                    "<tr>" +
                                     "<td width = '30%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:30px;text-align:left;font-weight:bold'>&nbsp;&nbsp;&nbsp;" +
                                            "</td>" +
                                        "<td valign = 'top' width = '50%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-size:18px;line-height:21px;text-align:left'>" +
                                            "<div style = 'font-weight:bold;font-size:18px;font-family:'Roboto',sans-serif'> Amount </div>" +
                                            "<span style = 'font-size:12px;color:#828282;font-family:'Roboto',sans-serif'> Cash On Delivery </span>" +
                                        "</td>" +
                                        "<td valign = 'top' width = '20%' style = 'padding-top:4%;padding-bottom:4%;border-collapse:collapse;font-family:'Roboto',sans-serif;font-weight:bold;font-size:18px;line-height:21px;text-align:right'><strong>₹" + TotalAmt.ToString("0.00") +
                                        "</strong></td>" +
                                    "</tr>" +
                                "</tbody>" +
                            "</table>";
                string subject = "Your order is placed successfully and it's on the way";
                SendEmail.EmailOrderBody(uObj.Name, uObj.Email, emailMsg, subject);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ChangeStatus(IEnumerable<long> Ids, bool status)
        {
            foreach (long id in Ids)
            {
                Banner c = db.Banners.Find(id);
                c.Status = status;
            }
            db.SaveChanges();
        }


        public IEnumerable<OrderListViewModel> GetUserOrder(int userid)
        {
            //OrderGridViewModel gridModel = new OrderGridViewModel();

            IQueryable<Order> lstOdt = null;

            lstOdt = db.Orders.Where(o => o.User_UserId == userid).OrderByDescending(o => o.OrderDt);

            List<OrderListViewModel> lstModel = new List<OrderListViewModel>();

            foreach (Order obj in lstOdt.ToList<Order>())
            {
                OrderListViewModel model = new OrderListViewModel();

                model.OrderId = obj.OrderId;
                model.OrderDt = obj.OrderDt.ToString("dd-MMM-yyyy");
                model.ShipName = obj.ShipName;
                model.GrandTotal = obj.TotalAmt;
                model.PaymentStatus = obj.PaymentStatus;
                model.OrderStatus = obj.OrderStatus;

                List<OrderDetailsViewModel> lstDetails = new List<OrderDetailsViewModel>();
                foreach (OrderDetail detObj in obj.OrderDetails)
                {
                    OrderDetailsViewModel detModel = new OrderDetailsViewModel();
                    detModel.OrderId = detObj.OrderDetId;
                    detModel.ProductId = detObj.ProductId;
                    detModel.ProductName = detObj.Product.Name;
                    detModel.Price = detObj.Price;
                    detModel.Qty = detObj.Qty;
                    detModel.Image = detObj.Product.Image1;
                    lstDetails.Add(detModel);
                }
                model.ProductList = lstDetails;

                lstModel.Add(model);
            }

            return lstModel;
        }


        public PayOrderViewModel GetOrderPaymentDetail(DeliveryDetViewModel model, string CouponCode, float DeliveryCrg, int uid, string UserId)
        {
            double DiscountAmt = 0.0;
            //Get User details
            User uObj = db.Users.Where(u => u.UserId == uid).FirstOrDefault();

            //Coupon details
            DiscountCoupon cObj = db.DiscountCoupons.Where(c => c.Code == CouponCode).FirstOrDefault();
            if (cObj != null)
                DiscountAmt = (model.SubTotal * cObj.DiscountPercent) / 100;
            else
                DiscountAmt = 0.0;

            double TotalAmt = model.GrandTotal - DiscountAmt + DeliveryCrg;

            PayOrderViewModel PayModel = new PayOrderViewModel();
            PayModel.Name = uObj.Name;
            PayModel.Email = uObj.Email;
            PayModel.Phone = uObj.ContactNo;
            PayModel.Amount = Convert.ToSingle(TotalAmt);

            return PayModel;
        }

        public OrderViewModel GetOrderDetailById(long oid, int uid)
        {
            Order obj = db.Orders.Where(o => o.OrderId == oid && o.User_UserId == uid).FirstOrDefault();

            OrderViewModel model = null;

            if (obj != null)
            {
                model = new OrderViewModel();
                model.BillAddress = obj.BillAddress;
                model.BillArea = obj.BillArea;
                model.BillCity = obj.BillCity;
                model.BillName = obj.BillName;
                model.BillPhone = obj.BillPhone;
                model.BillPinCode = obj.BillPinCode;
                model.CouponId = obj.CouponId;
                model.DeliveryCharges = obj.DeliveryCharges;
                model.DiscountAmt = obj.DiscountAmt;
                model.Email = obj.Email;
                model.IsPaid = obj.PaymentStatus;
                model.OrderDt = obj.OrderDt;
                model.OrderId = obj.OrderId;
                model.PayMode = obj.PayMode;
                model.PayRemark = obj.PayRemark;
                model.ShipAddress = obj.ShipAddress;
                model.ShipArea = obj.ShipArea;
                model.ShipCity = obj.ShipCity;
                model.ShipName = obj.ShipName;
                model.ShipPhone = obj.ShipPhone;
                model.ShipPinCode = obj.ShipPinCode;
                model.TaxAmt = obj.TaxAmt;
                model.TotalAmt = obj.TotalAmt;
                model.OrderStatus = obj.OrderStatus;

                List<OrderDetailsViewModel> lstDetails = new List<OrderDetailsViewModel>();
                foreach (OrderDetail detObj in obj.OrderDetails)
                {
                    OrderDetailsViewModel detModel = new OrderDetailsViewModel();
                    detModel.ProductName = detObj.Product.Name;
                    detModel.Price = detObj.Price;
                    detModel.Qty = detObj.Qty;
                    detModel.Image = detObj.Product.Image1;
                    lstDetails.Add(detModel);
                }
                model.ProductList = lstDetails;

                return model;
            }
            else
                return null;
        }


        public int OrderCnt()
        {
            int orderCnt = db.Orders.Where(o => o.PaymentStatus == true).Count();
            return orderCnt;
        }

        public float TotalCollection()
        {
            float totalCollection = db.Orders.Where(o => o.PaymentStatus == true).Sum(o => o.TotalAmt);
            return totalCollection;
        }

        public CountViewModel TodaysOrderCount()
        {
            DateTime currDate = DateTime.Now.Date;
            CountViewModel model = new CountViewModel();
            model.CompletedOrders = db.Orders.Where(o => o.OrderStatus == "Completed" && o.OrderDt.Year == currDate.Year && o.OrderDt.Day == currDate.Day && o.OrderDt.Month == currDate.Month).Count();
            model.PendingOrders = db.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderDt.Year == currDate.Year && o.OrderDt.Day == currDate.Day && o.OrderDt.Month == currDate.Month).Count();
            model.TodaysOrders = model.CompletedOrders + model.PendingOrders;

            return model;


        }

        public OrderGridViewModel GetAllStatusOrder(string sort, string sortDir, int skipRecords, int pgSize, string searchStr)
        {
            //create a object of 'RETURN TYPE I.E. OF CategoryGridViewModel'
            OrderGridViewModel gridModel = new OrderGridViewModel();

            IQueryable<Order> lstOrder = null;

            //search code start**********************************************************
            if (string.IsNullOrEmpty(searchStr))
            {
                lstOrder = db.Orders.Where(o => o.OrderStatus != "Completed").OrderByDescending(o => o.OrderId);
            }
            else
            {
                lstOrder = db.Orders.Where(o => o.OrderStatus != "Completed" && o.ShipName.ToLower().Contains(searchStr)).OrderByDescending(o => o.OrderId);
            }
            //search code end************************************************************

            //sort code start************************************************************
            switch (sort)
            {
                case "shipname":
                    lstOrder = sortDir == "asc" ? lstOrder.OrderBy(o => o.ShipName) : lstOrder.OrderByDescending(o => o.OrderId);
                    break;
                case "orderdt":
                    lstOrder = sortDir == "asc" ? lstOrder.OrderBy(o => o.OrderDt) : lstOrder.OrderByDescending(o => o.OrderId);
                    break;
                default:
                    lstOrder = lstOrder.OrderBy(o => o.OrderId).OrderByDescending(o => o.OrderId);
                    break;
            }
            //sort code end**************************************************************

            //assign total no. of records
            gridModel.TotalRecords = lstOrder.Count();

            //pagination code start******************************************************
            lstOrder = lstOrder.Skip(skipRecords).Take(pgSize);
            //pagination code end********************************************************

            //assign lstCategory to CategoryList in gridModel i.e. CategoryGridViewModel
            List<OrderListViewModel> lstModel = new List<OrderListViewModel>();

            foreach (Order obj in lstOrder.ToList<Order>())
            {
                OrderListViewModel model = new OrderListViewModel();

                model.OrderId = obj.OrderId;
                model.OrderDt = obj.OrderDt.ToString("dd-MMM-yyyy");
                model.ShipName = obj.ShipName;
                model.GrandTotal = obj.TotalAmt;
                model.OrderStatus = obj.OrderStatus;
                lstModel.Add(model);
            }

            gridModel.OrderList = lstModel;
            return gridModel;
        }

        public bool OrderConfirmation(OrderConfirmViewModel model)
        {
            try
            {
                //Get User details
                User uObj = db.Users.Where(u => u.UserId == model.UserId).FirstOrDefault();

                IQueryable<TempCart> lstCart = null;
                lstCart = db.TempCarts.Where(t => t.UserId == model.UserId);

                Order obj = new Order();
                OrderDetail objOdet = new OrderDetail();

                obj.OrderId = NewId(); //Id generated manually
                obj.BillAddress = model.BillAddress;
                obj.BillArea = model.BillArea;
                obj.BillCity = model.BillCity;
                obj.BillName = uObj.Name;
                obj.BillPhone = uObj.ContactNo;
                obj.BillPinCode = model.BillPinCode;
                obj.CouponId = model.CouponId;
                obj.DeliveryCharges = model.DeliveryCharges;
                obj.DiscountAmt = model.DiscountAmt;
                obj.Email = uObj.Email;
                obj.PaymentStatus = model.PaymentStatus;
                obj.OrderDt = DateTime.Now;
                obj.PayMode = 1;
                obj.PayRemark = "Paid";
                obj.ShipAddress = model.ShipAddress;
                obj.ShipArea = model.ShipArea;
                obj.ShipCity = model.ShipCity;
                obj.ShipName = model.ShipName;
                obj.ShipPhone = model.ShipPhone;
                obj.ShipPinCode = model.ShipPinCode;
                obj.User_UserId = uObj.UserId;
                obj.PaymentId = model.PaymentId;
                obj.PaymentDate = DateTime.Now;
                if (model.PaymentStatus == true)
                    obj.OrderStatus = "Confirmed";
                else
                    obj.OrderStatus = "Cancelled";

                obj.TaxAmt = 0;
                obj.TotalAmt = Convert.ToSingle(model.TotalAmt);
                db.Orders.Add(obj);

                long NeworderDetailId = NewOrderDetId();

                foreach (TempCart tModel in lstCart.ToList<TempCart>())
                {

                    objOdet = new OrderDetail();
                    objOdet.OrderDetId = NeworderDetailId;
                    objOdet.OrderId = obj.OrderId;
                    objOdet.Price = Convert.ToSingle(tModel.Price);
                    objOdet.Qty = tModel.Quantity;
                    objOdet.ProductId = tModel.ProductId;
                    db.OrderDetails.Add(objOdet);
                    NeworderDetailId++;
                }

                //Remove Temperory Cart
                IEnumerable<TempCart> tObj = db.TempCarts.Where(t => t.UserId == model.UserId);
                if (tObj != null)
                {
                    foreach (TempCart tc in tObj)
                    {
                        db.TempCarts.Remove(tc);
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ChangeOrderStatus(IEnumerable<long> Ids, string status)
        {
            foreach (long id in Ids)
            {
                Order c = db.Orders.Find(id);
                c.OrderStatus = status;
            }
            db.SaveChanges();

        }

        public IEnumerable<BillDetailViewModel> GenerateBill(long oid)
        {
            IEnumerable<BillDetailViewModel> lstModel = db.Database.SqlQuery<BillDetailViewModel>(String.Format("Select * From GetBillDetail Where OrderId={0}", oid));
            return lstModel;
        }
    }
}
