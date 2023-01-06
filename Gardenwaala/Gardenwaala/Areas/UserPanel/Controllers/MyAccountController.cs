using BAL;
using Gardenwaala.Areas.UserPanel.Models;
using Gardenwaala.CustomSecurity;
using GardenViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Gardenwaala.Areas.UserPanel.Controllers
{
    public class MyAccountController : Controller
    {
        public ActionResult Login()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            if (currentUser == null)
            {
                return View();
            }
            else
                return RedirectToAction("MyProfile");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(FormCollection formData)
        {
            IUserRepository repoUser = new UserRepository();
            LoginViewModel model = new LoginViewModel();
            if (TryUpdateModel(model, formData))
            {
                CurrentUserViewModel uObj = repoUser.Validate(model.Username, Crypto.SHA1(model.Password));
                if (uObj != null)
                {
                    //at 8th position user email is stored which required, if user is super admin, and want to access the a/c of user
                    string data = string.Format("{0}|{1}|{2}|{3}", uObj.UserId, uObj.Name, uObj.Email, DateTime.Now);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, uObj.Name, DateTime.Now, DateTime.Now.AddMinutes(10), false, data);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(cookie);
                    return Redirect(Request.UrlReferrer.ToString());
                    //                    return RedirectToAction("MyProfile");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View();
                }

            }
            else
            {
                if (string.IsNullOrEmpty(model.Username))
                    ModelState.AddModelError("Username", "Username is Blank");
                if (string.IsNullOrEmpty(model.Password))
                    ModelState.AddModelError("Password", "Password is Blank");

                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(FormCollection formData)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Name = formData["Name"];
            model.ContactNo = formData["ContactNo"];
            model.Email = formData["Email"];
            model.Password = formData["Password"];
            model.CreatedDt = DateTime.Now;
            string actualPassword = model.Password;
            model.Password = Crypto.SHA1(model.Password);
            IUserRepository repoUser = new UserRepository();
            if (repoUser.Register(model))
            {
                //EMAIL to UserPanel
                string emailMsg = "Your account has been created – now it will be easier to manage your task by yourself from anywhere and anytime. Login with following details, when your account is activated.<br/><b>Username: </b>" + model.Email + "<br/><b>Password :</b>" + actualPassword;
                string subject = "Welcome to Gardenwaala.com - Registered Successfully.";
                SendEmail.EmailBody(model.Name, model.Email, emailMsg, subject);

                //Email to Gardenwaala.com
                emailMsg = "New user is registered to Gardenwaala.com– Following are the details of the user. Name:" + model.Name + " .<br/><b>Email Id: </b>" + model.Email + "<br/><b>Contact No :</b>" + model.ContactNo;
                subject = "New user registration at Gardenwaala.com";
                //SendEmail.EmailBody("Gardenwaala.com", "support@gardenwaala.com", emailMsg, subject);

                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                ModelState.AddModelError("", "Error, Please try again!");
                return View();
            }

        }

        public ActionResult ForgotPwd()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgotPwd(OnlyEmailViewModel model)
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            string newPassword = RandomPassword.Generate();
            string encNewPassword = Crypto.SHA1(newPassword);
            IUserRepository repoUser = new UserRepository();

            if (repoUser.ResetPassword(model.Email, encNewPassword))
            {
                //EMAIL to UserPanel
                string emailMsg = "Your password has been changed successfully – Please login with the new password.<br/><b>Username: </b>" + model.Email + "<br/><b>Password :</b>" + newPassword;
                string subject = "Password Reset Successfully";
                SendEmail.EmailBody("User", model.Email, emailMsg, subject);

                return Json(new { status = true, msg = "Thank you for Registration, We will get back to you soon" });
            }
            else
            {
                return Json(new { status = false, msg = "Error, Please try again!" });

            }
        }

        [CustomClientAuthentication]
        public ActionResult MyProfile()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            UserViewModel model = repoUser.GetById(currentUser.UserID);

            return View(model);
        }

        [CustomClientAuthentication]
        public ActionResult MyAddress()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            UserViewModel model = repoUser.GetById(currentUser.UserID);

            return View(model);
        }

        [CustomClientAuthentication]
        public ActionResult MyOrders()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            UserViewModel model = repoUser.GetById(currentUser.UserID);
            IOrderRepository repoOrder = new OrderRepository();
            IEnumerable<OrderListViewModel> lstModel = repoOrder.GetUserOrder(currentUser.UserID);

            return View(lstModel);
        }


        [CustomClientAuthentication]
        public PartialViewResult MyRecentOrder()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;

            IOrderRepository repoOrder = new OrderRepository();
            IEnumerable<OrderListViewModel> lstModel = repoOrder.GetUserOrder(currentUser.UserID);

            return PartialView("_MyRecentOrder", lstModel);
        }


        [CustomClientAuthentication]
        public PartialViewResult UserInfo()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            UpdateUserViewModel model = repoUser.GetUserById(currentUser.UserID);
            return PartialView("_UserInfo", model);
        }

        [CustomClientAuthentication]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [CustomClientAuthentication]
        public PartialViewResult ChangePasswd()
        {
            return PartialView("_ChangePasswd");
        }

        [HttpPost]
        public ActionResult ChangePwd(ChangePasswordViewModel model)
        {
            string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            model.OldPassword = Crypto.SHA1(model.OldPassword);
            model.NewPassword = Crypto.SHA1(model.NewPassword);
            model.Username = currentUser.Email;
            IUserRepository repoUser = new UserRepository();

            if (repoUser.ChangePassword(model))
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                    Response.Redirect(ReturnUrl);
                else
                    return RedirectToAction("MyProfile");
            }
            else
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateProfile(UpdateUserViewModel model)
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            model.UserId = currentUser.UserID;
            if (repoUser.Update(model))
            {
                return RedirectToAction("MyProfile");
            }
            else
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View();
        }

        public RedirectToRouteResult Signout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult OrderDetail(long oid)
        {

            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            int userid = currentUser.UserID;
            IOrderRepository repoOrder = new OrderRepository();
            OrderViewModel lstModel = repoOrder.GetOrderDetailById(oid, userid);

            return View(lstModel);
        }

        public PartialViewResult BestSeller()
        {
            IProductRepository repoFMenu = new ProductRepository();
            IEnumerable<ProductViewModel> lstModel = repoFMenu.GetBestSeller(4);
            return PartialView("_BestSeller", lstModel);
        }

        public PartialViewResult MyAccountLeftMenu()
        {
            CustomPrincipal currentUser = HttpContext.User as CustomPrincipal;
            IUserRepository repoUser = new UserRepository();
            UserViewModel model = repoUser.GetById(currentUser.UserID);
            return PartialView("_MyAccountLeftMenu", model);
        }
    }
}