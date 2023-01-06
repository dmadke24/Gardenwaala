using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using BAL;
using GardenViewModel;
using GardenWaalaAPI.Models;
using GardenWaalaAPI.Security;
using GardenAPIViewModel;

namespace GardenWaalaAPI.Controllers
{
    [RoutePrefix("GardenWaalaAPI/account")]
    public class AccountController : ApiController
    {
        IUserRepository repoUser;

        public AccountController()
        {
            repoUser = new UserRepository();
        }

        //this method is called when client will login
        [Route("Validate")]
        [HttpPost]
        public IHttpActionResult Validate(LoginViewModel model)
        {
            CurrentUserWebApiViewModel apiModel = new CurrentUserWebApiViewModel();
            CurrentUserViewModel uObj = repoUser.Validate(model.Username, Crypto.SHA1(model.Password));
            if (uObj != null)
            {
                apiModel.msg = "Validate Successfully.";
                apiModel.status = true;
                apiModel.data = uObj;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "Invalid Credentials.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("UpdateProfile")]
        [HttpPost]
        public IHttpActionResult UpdateProfile(UpdateUserViewModel Model)
        {
            UpdateUserWebApiModel apiModel = new UpdateUserWebApiModel();
            Model.UserId = (System.Threading.Thread.CurrentPrincipal as CustomPrincipal).UserId;
            if (ModelState.IsValid && repoUser.UpdateUserProfile(Model))
            {
                apiModel.msg = "Profile Updated Successfully.";
                apiModel.status = true;
                apiModel.data = null;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "Error In Updating Record.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("ChangePassword")]
        [HttpPut]
        public IHttpActionResult ChangePassword(ChangePasswordViewModel model)
        {

            ChangePasswordWebApiViewModel apiModel = new ChangePasswordWebApiViewModel();

            if (ModelState.IsValid)
            {
                string newPass = model.NewPassword;
                model.OldPassword = Crypto.SHA1(model.OldPassword);
                model.NewPassword = Crypto.SHA1(model.NewPassword);
                if (repoUser.ChangePassword(model))
                {
                    string authInfo;
                    authInfo = model.Username + ":" + newPass;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    this.ActionContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                    //Change password Email and SMS

                    string Uname = (System.Threading.Thread.CurrentPrincipal as CustomPrincipal).Username;
                    string Uemail = (System.Threading.Thread.CurrentPrincipal as CustomPrincipal).Email;
                    string Contact = (System.Threading.Thread.CurrentPrincipal as CustomPrincipal).Contact;


                    //EMAIL
                    string changPassEmailMsg = "Your Password changed successfully. Your changed password is <b>" + model.NewPassword + "</b>";
                    string subject = "Password Changed successfully of Gardenwaala panel";
                    SendEmail.EmailBody(Uname, Uemail, changPassEmailMsg, subject);


                    //SMS
                    string changPassSmsMsg = "Your Password changed successfully. Your changed password is " + model.NewPassword;
                    // SendSMS(Uname, Contact, changPassSmsMsg);

                    apiModel.msg = "Password Changed successfully.";
                    apiModel.status = true;
                    apiModel.data = null;
                    return Ok(apiModel);
                }
                else
                {
                    apiModel.msg = "Error in changing password.";
                    apiModel.status = false;
                    apiModel.data = null;
                    return Ok(apiModel);
                }
            }
            else
            {
                apiModel.msg = "Not a valid record.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [Route("CreateUser")]
        [HttpPost]
        public IHttpActionResult CreateUser(RegisterViewModel model)
        {
            RegisterWebApiViewModel apiModel = new RegisterWebApiViewModel();
            string actualPassword = model.Password;
            model.Password = Crypto.SHA1(model.Password);
            IUserRepository repoUser = new UserRepository();
            if (repoUser.RegisterUser(model))
            {
                //EMAIL to CLIENT
                string emailMsg = "Your account has been created – now it will be easier to manage your task by yourself from anywhere and anytime. Login with following details, when your account is activated.<br/><b>Username: </b>" + model.Email + "<br/><b>Password :</b>" + actualPassword;
                string subject = "Welcome to Gardenwaala";
                SendEmail.EmailBody(model.Name, model.Email, emailMsg, subject);

                //Email to Gardenwaala
                emailMsg = "New user is registered to Gardenwaala– Following are the details of the user. Name:" + model.Name + " .<br/><b>Email Id: </b>" + model.Email + "<br/><b>Contact No :</b>" + model.ContactNo;
                subject = "New user registration at Gardenwaala";
                SendEmail.EmailBody(model.Name, model.Email, emailMsg, subject);

                //SMS
                string smsMsg = string.Format("Welcome to Gardenwaala, Thank you for Registration, your login details are, Username: {0}, Password: {1}", model.Email, actualPassword);
                //SendSMS(model.Name, model.ContactNo, smsMsg);

                apiModel.msg = "Registered Successfully.";
                apiModel.status = true;
                apiModel.data = null;
                return Ok(apiModel);
            }
            else
            {
                apiModel.msg = "Can't Register, Email Id Already exist.";
                apiModel.status = false;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("Forgot")]
        [HttpPost]
        public IHttpActionResult Forgot(OnlyEmailViewModel model)
        {
            ForgotPasswdWebApiViewModel apiModel = new ForgotPasswdWebApiViewModel();
            string usernameContact = repoUser.GetNameAndContactByEmail(model.Email);
            if (usernameContact != null)
            {
                string[] userDetails = usernameContact.Split('|');

                string newPassword = RandomPassword.Generate();
                string encNewPassword = Crypto.SHA1(newPassword);
                if (repoUser.ResetPassword(model.Email, encNewPassword))
                {
                    //send email
                    string forgotEmailMsg = "Your Password is reset successfully. Your new password is <b>" + newPassword + "</b>";
                    string subject = "Password Reset Successfully of Bulkit Panel";
                    SendEmail.EmailBody(userDetails[0], model.Email, forgotEmailMsg, subject);

                    //Send SMS
                    string forgotSmsMsg = "Your Password is reset successfully. Your new password is " + newPassword;
                    // SendSMS(userDetails[0], userDetails[1], forgotSmsMsg);

                    apiModel.msg = "Password reset successfully.";
                    apiModel.status = true;
                    apiModel.data = null;
                    return Ok(apiModel);
                }
                else
                {

                    apiModel.msg = "Can't Reset Password, Try again.";
                    apiModel.status = true;
                    apiModel.data = null;
                    return Ok(apiModel);
                }
            }
            else
            {
                apiModel.msg = "Invalid Email, it is not registered.";
                apiModel.status = true;
                apiModel.data = null;
                return Ok(apiModel);
            }
        }

        [CustomAuthorization(Roles = "C")]
        [Route("Profile")]
        [HttpGet]
        public IHttpActionResult Profile()
        {
            UserProfileWebApiViewModel apiModel = new UserProfileWebApiViewModel();
            int UserId = (System.Threading.Thread.CurrentPrincipal as CustomPrincipal).UserId;
            IUserRepository repoUser = new UserRepository();
            UserViewModel model = repoUser.GetById(UserId);

            apiModel.msg = "Registered Successfully.";
            apiModel.status = true;
            apiModel.data = model;
            return Ok(apiModel);
        }
    }
}
