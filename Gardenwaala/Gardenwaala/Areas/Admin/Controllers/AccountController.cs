using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.Net.Http;
using Gardenwaala.CustomSecurity;
using System.IO;
using GardenViewModel;
using BAL;

namespace Gardenwaala.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        IUserRepository repoUser;
        IOrderRepository repoOrder;
        public AccountController()
        {
            repoUser = new UserRepository();
            repoOrder = new OrderRepository();
        }

        public ViewResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(FormCollection formData)
        {
            LoginViewModel model = new LoginViewModel();
            if (TryUpdateModel(model, formData))
            {
                CurrentUserViewModel uObj = repoUser.Validate(model.Username, Crypto.SHA1(model.Password));
                if (uObj != null)
                {
                    string data = string.Format("{0}|{1}|{2}|{3}", uObj.UserId, uObj.Name, uObj.Email, DateTime.Now);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, uObj.Name, DateTime.Now, DateTime.Now.AddMinutes(10), false, data);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Dashboard");
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

        public ViewResult Unauthorize()
        {
            return View();
        }

        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ViewResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "Email-Id is Blank");
            return View("Login");
        }



        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ViewResult ChangePassword(ChangePasswordViewModel model)
        {
            if (repoUser.ChangePassword(model))
                ViewBag.changeStatus = true;
            else
                ViewBag.changeStatus = false;

            return View();
        }
    }
}