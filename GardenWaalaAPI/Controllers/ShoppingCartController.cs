using BAL;
using GardenWaalaAPI.Models;
using GardenWaalaAPI.Security;
using GardenViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace GardenWaalaAPI.Controllers
{
    [RoutePrefix("GardenWaalaAPI/shoppingcart")]
    public class ShoppingCartController : ApiController
    {
        [CustomAuthorization(Roles = "C")]
        [Route("AddToTempCart")]
        [HttpPost]
        public IHttpActionResult AddToTempCart(AddToTempCartViewModel cartModel)
        {

            ITempCartRepository repoTCart = new TempCartRepository();
            TempCartViewModel model = new TempCartViewModel();
            model.UserId = cartModel.UserId;
            model.Price = cartModel.price;
            model.Quantity = cartModel.qty;
            model.ProductId = cartModel.prodid;
            IEnumerable<TempCartViewModel> lstModel = repoTCart.Save(model);
            if (lstModel != null)
            {
                return Ok(lstModel);
            }
            else
            {
                return BadRequest("Can't Add Product to Cart.");
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("detailcart/{uid:int}")]
        [HttpGet]
        public IHttpActionResult DetialCart(int uid)
        {
            ITempCartRepository repoTCart = new TempCartRepository();
            IEnumerable<TempCartViewModel> lstModel = repoTCart.GetMyCart(uid);
            if (lstModel != null)
            {
                return Ok(lstModel);
            }
            else
            {
                return BadRequest("Error in Cart.");
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("DeleteCartItem/{TempId:int}/{UserId:int}")]
        [HttpGet]
        public IHttpActionResult DeleteCartItem(int TempId, int UserId)
        {
            ITempCartRepository repoTCart = new TempCartRepository();
            if (repoTCart.DeleteCartItem(TempId, UserId))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Error in Cart.");
            }
        }

        //[CustomAuthorization(Roles = "C")]
        //[Route("AddToTempCart")]
        //[HttpPost]
        //public IHttpActionResult BillingAddress(AddressViewModel cartModel)
        //{
        //    ITempCartRepository repoTCart = new TempCartRepository();
        //    TempCartViewModel model = new TempCartViewModel();
        //    model.UserId = cartModel.UserId;
        //    model.Price = cartModel.price;
        //    model.Quantity = cartModel.qty;
        //    model.FoodId = cartModel.prodid;
        //    IEnumerable<TempCartViewModel> lstModel = repoTCart.Save(model);
        //    if (lstModel != null)
        //    {
        //        return Ok(lstModel);
        //    }
        //    else
        //    {
        //        return BadRequest("Can't Add Product to Cart.");
        //    }
        //}

        [CustomAuthorization(Roles = "C")]
        [Route("DeliveryArea/{Pincode:int}")]
        [HttpGet]
        public IHttpActionResult DeliveryArea(string Pincode)
        {
            IAreaRepository repoArea = new AreaRepository();
            if (repoArea.DeliveryArea(Pincode))
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("OrderConfirmation")]
        [HttpPost]
        public IHttpActionResult OrderConfirmation(OrderConfirmViewModel cartModel)
        {
            IOrderRepository repoOrder = new OrderRepository();
            if (repoOrder.OrderConfirmation(cartModel))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Error In Order");
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("UserOrder/{UserId:int}")]
        [HttpGet]
        public IHttpActionResult UserOrder(int UserId)
        {
            IOrderRepository repoOrder = new OrderRepository();
            IEnumerable<OrderListViewModel> lstModel = repoOrder.GetUserOrder(UserId);
            if (lstModel != null)
            {
                return Ok(lstModel);
            }
            else
            {
                return BadRequest("No Order Found.");
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("UserOrderDetail/{oid:long}/{UserId:int}")]
        [HttpGet]
        public IHttpActionResult UserOrderDetail(long oid, int UserId)
        {
            IOrderRepository repoOrder = new OrderRepository();
            OrderViewModel lstModel = repoOrder.GetOrderDetailById(oid, UserId);
            if (lstModel != null)
            {
                return Ok(lstModel);
            }
            else
            {
                return BadRequest("No Order Found.");
            }
        }

    }
}
