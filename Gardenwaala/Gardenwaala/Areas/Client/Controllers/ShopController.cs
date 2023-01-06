using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using GardenViewModel;
using System.Web.Helpers;
using System.Web.Security;
using Gardenwaala.CustomSecurity;
using System.Net.Mail;
using System.Net;
using System.Text;
using Gardenwaala.Areas.Client.Models;
using Razorpay.Api;

namespace Gardenwaala.Areas.Client.Controllers
{
    public class ShopController : Controller
    {
        public static int i = 1;
        public ActionResult ProductList(int pid, int subid = 0)
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetHomeProduct(pid, subid);
            ViewBag.CurrentMenuid = pid;
            return View(lstModel);
        }

        public ActionResult ProductListCategory(int pid, int subid = 0)
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetHomeProductCat(pid, subid);
            ViewBag.CurrentMenuid = pid;
            return View(lstModel);
        }

        public PartialViewResult SubTypeMenu(int pid)
        {
            ISubCategoryRepository repoSubtype = new SubCategoryRepository();
            IEnumerable<SubCategoriesAPIViewModel> lststype = repoSubtype.GetSubCategory(pid);
            return PartialView("_SubTypeMenu", lststype);
        }

        public PartialViewResult LeftCategories()
        {
            ICategoryRepository repoCat = new CategoryRepository();
            IEnumerable<CategoryViewModel> lstFtype = repoCat.GetCategory();
            return PartialView("_LeftCategories", lstFtype);
        }

        public PartialViewResult LeftCoupon()
        {
            IDiscountCouponRepository repoDisc = new DiscountCouponRepository();
            IEnumerable<DiscountCouponViewModel> lstModel = repoDisc.HomeCouponImage();
            return PartialView("_LeftCoupon", lstModel);
        }

        public ActionResult ProductDetail(int pid)
        {
            IProductRepository repoFmenu = new ProductRepository();
            ProductViewModel model = repoFmenu.GetById(pid);

            return View(model);
        }

        public PartialViewResult RelatedProducts(int pid)
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetRelatedProduct(pid);
            return PartialView("_RelatedProducts", lstModel);
        }

        [CustomClientAuthentication]
        public ActionResult ShoppingCart()
        {
            List<TempCartViewModel> lstCart = null;
            if (Session["cart"] == null)
            {
                lstCart = new List<TempCartViewModel>();
            }
            else
            {
                lstCart = Session["cart"] as List<TempCartViewModel>;
            }
            return View(lstCart);
        }

        [HttpPost]
        public PartialViewResult AddToTempCart(int prodid, int qty, float price)
        {

            IProductRepository repoProduct = new ProductRepository();
            ProductViewModel modelProduct = repoProduct.GetById(prodid);
            List<TempCartViewModel> lstCart = null;

            if (modelProduct != null)
            {
                TempCartViewModel modelCartItem = new TempCartViewModel();
                modelCartItem.Price = price;
                modelCartItem.Quantity = qty;
                modelCartItem.ProductId = prodid;
                modelCartItem.ProdName = modelProduct.Name;
                modelCartItem.Image = modelProduct.Image1;
                modelCartItem.TempId = i;
                i++;
                if (Session["cart"] == null)
                {
                    lstCart = new List<TempCartViewModel>();
                }
                else
                {
                    lstCart = Session["cart"] as List<TempCartViewModel>;
                }

                lstCart.Add(modelCartItem);
                Session["cart"] = lstCart;
            }
            return PartialView("_ShoppingCart", lstCart);
        }

        public PartialViewResult Cart()
        {
            List<TempCartViewModel> lstCart = null;
            if (Session["cart"] != null)
            {
                lstCart = Session["cart"] as List<TempCartViewModel>;
            }
            return PartialView("_ShoppingCart", lstCart);
        }

        [HttpPost]
        public JsonResult CartCount()
        {
            int prodCnt = 0;
            List<TempCartViewModel> lstCart = null;
            if (Session["cart"] != null)
            {
                lstCart = Session["cart"] as List<TempCartViewModel>;
                prodCnt = lstCart.Count;
            }
            return Json(prodCnt);
        }


        [HttpGet]
        public JsonResult GetMyCart() //Temperory header cart
        {
            List<TempCartViewModel> lstCart = null;
            if (Session["cart"] == null)
            {
                lstCart = new List<TempCartViewModel>();
            }
            else
            {
                lstCart = Session["cart"] as List<TempCartViewModel>;
            }
            Session["cart"] = lstCart;
            if (lstCart != null)
            {
                return Json(lstCart, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = false, Msg = "Error in adding your data! Please try again." });
            }
        }

        public ActionResult DetailCart(int UserId) //detail cart list page
        {
            ITempCartRepository repoTCart = new TempCartRepository();
            IEnumerable<TempCartViewModel> lstModel = repoTCart.GetMyCart(UserId);
            return View(lstModel);
        }

        [CustomClientAuthentication]
        public ActionResult Billing() //validate user for billing
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            ViewBag.UserAddr = repoUser.GetUserAddr(currentUser.UserID);

            return View();
        }

        [HttpPost]
        public ActionResult Billing(OrderViewModel model)
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IOrderRepository repoOrder = new OrderRepository();
            IEnumerable<TempCartViewModel> tempCart = new List<TempCartViewModel>();

            if (Session["cart"] != null)
            {
                tempCart = Session["cart"] as IEnumerable<TempCartViewModel>;
            }

            repoOrder.Add(model, currentUser.UserID, tempCart);
            Session.Remove("cart");

            return RedirectToAction("Index", "Home");
        }

        public PartialViewResult CartSummery()
        {
            List<TempCartViewModel> lstCart = null;
            if (Session["cart"] != null)
            {
                lstCart = Session["cart"] as List<TempCartViewModel>;
            }
            return PartialView("_CartSummery", lstCart);
        }


        //**************These methods may need in after implementing Payment Gateway*********************

        /*[CustomClientAuthentication]
        public ActionResult DeliveryDet()
        {
            int UserId = 0;
            DeliveryDetViewModel model = new DeliveryDetViewModel();
            ITempCartRepository repoTCart = new TempCartRepository();
            //Live key: rzp_live_SgZigD1JKBCvf9
            //Live Secret: eJTkgaIiFCP9Ar3rNpLjeMLi

            //Test Key: rzp_test_wFpHCEYmDSVyZ7
            //Test Secret: oiXgfpejASUZpyfusTCNPmTk

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", 100); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", "12121");
            input.Add("payment_capture", 1);

            string key = "rzp_live_SgZigD1JKBCvf9";
            string secret = "eJTkgaIiFCP9Ar3rNpLjeMLi";

            //RazorpayClient client = new RazorpayClient(key, secret);

            //Razorpay.Api.Order order = client.Order.Create(input);
            //model.OrderId = order["id"].ToString();

            model.DeliveryAddress = TempData.Peek("AddrDet") as AddressViewModel;
            model.TempCart = repoTCart.GetMyCart(UserId);

            return View(model);
        }

        public JsonResult ValidateCoupon(string couponCode)
        {
            IDiscountCouponRepository repoDiscnt = new DiscountCouponRepository();
            float discountPer = repoDiscnt.ValidateCoupon(couponCode);
            if (discountPer != null)
            {
                return Json(discountPer);
            }
            else
                return Json(new { Status = false, Msg = "Error in adding your data! Please try again." });
        }

        [CustomClientAuthentication]
        [HttpPost]
        public ActionResult ConfirmOrder(FormCollection data)
        {
            int UserId = 0;
            //pay amount and then save record

            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;

            DeliveryDetViewModel model = new DeliveryDetViewModel();
            ITempCartRepository repoTCart = new TempCartRepository();
            IOrderRepository repoOrder = new OrderRepository();


            string CouponCode = data["CouponCode"];
            float DeliveryCrg = Convert.ToSingle(data["DeliveryCrg"]);

            model.DeliveryAddress = TempData.Peek("AddrDet") as AddressViewModel;
            model.TempCart = repoTCart.GetMyCart(UserId);
            string uids = "";
            PayOrderViewModel PayModel = repoOrder.GetOrderPaymentDetail(model, CouponCode, DeliveryCrg, currentUser.UserID, uids);

            return View("Payment");
        }

        public MvcHtmlString PreparePOSTForm(string url, System.Collections.Hashtable data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return new MvcHtmlString(strForm.ToString() + strScript.ToString());
        }


        public ActionResult OrderSuccess()
        {
            //CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;

            //DeliveryDetViewModel model = new DeliveryDetViewModel();
            //ITempCartRepository repoTCart = new TempCartRepository();
            //IOrderRepository repoOrder = new OrderRepository();


            //string CouponCode = data["CouponCode"];
            //float DeliveryCrg = Convert.ToSingle(data["DeliveryCrg"]);

            //model.DeliveryAddress = TempData.Peek("AddrDet") as AddressViewModel;
            //model.TempCart = repoTCart.GetMyCart(UserId);

            //System.Collections.Hashtable ParamData = PayuMoney.GenerateParams();

            //MvcHtmlString htmlForm = PreparePOSTForm("https://test.payu.in/_payment", ParamData);

            //if (repoOrder.Add(model, CouponCode, DeliveryCrg, currentUser.UserID, UserId))
            //{
            //    return View();
            //}
            //else
            //{
            //    return View();
            //}
            return View();
        }

        public ActionResult OrderFail()
        {
            return View();
        }*/

        [CustomClientAuthentication]
        [HttpGet]
        public ActionResult DeleteCartItem(int TempId)
        {
            List<TempCartViewModel> lstCart = new List<TempCartViewModel>();
            lstCart = Session["cart"] as List<TempCartViewModel>;
            var itemToRemove = lstCart.Single(r => r.TempId == TempId);
            lstCart.Remove(itemToRemove);
            return RedirectToAction("ShoppingCart");

        }
    }
}