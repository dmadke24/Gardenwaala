using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Gardenwaala.Areas.UserPanel.Models
{
    public static class Communication
    {

        public static void SendEMail(string name, string email, string contact, string subject, string desc)
        {
            /**********************send email and sms to the Wesite who has enquired**********************/
            try
            {
                //*************************Email To the Company******************************************
                var body = SendEmail.EmailEnqBody("Gardenwaala", "You got enquiry from the user <b>Name:" + name + "<br />Email:" + email + "<br />Phone:" + contact + "<br />Subject:" + subject + "<br />Message:" + desc + "<br /></b>");

                var message = new MailMessage();
                message.To.Add("support@gardenwaala.com");
                message.From = new MailAddress("support@gardenwaala.com", "Gardenwaala");  // replace with valid value
                message.Subject = "Enquiry from your website";
                message.Body = string.Format(body, "Gardenwaala", "support@gardenwaala.com");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "webmail.gardenwaala.com";
                    NetworkCredential NetworkCred = new NetworkCredential("support@gardenwaala.com", "wp#P1r77");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(message);
                }

                //*************************Email To the UserPanel******************************************
                body = SendEmail.EmailEnqBody(name, "Thanks for contacting Gardenwaala, Our support team will get back to you soon.");

                message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.From = new MailAddress("support@gardenwaala.com", "Gardenwaala");  // replace with valid value
                message.Subject = "Thanks for contacting Gardenwaala";
                message.Body = string.Format(body, "Gardenwaala", "support@gardenwaala.com");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "webmail.gardenwaala.com";
                    NetworkCredential NetworkCred = new NetworkCredential("support@gardenwaala.com", "wp#P1r77");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool SendFeedbackEmail(string name, string email, string contact, string subject, string desc)
        //{
        //    try
        //    {
        //        var body = SendEmail.EmailBody("Gardenwaala", "You got Enquiry from the UserPanel <b>Name:" + name + "<br />Email:" + email + "<br />Phone:" + contact + "<br />Subject:" + subject + "<br />Message:" + desc + "<br /></b>");

        //        var message = new MailMessage();
        //        message.To.Add("support@gardenwaala.com");
        //        message.From = new MailAddress("support@gardenwaala.com", "Gardenwaala");  // replace with valid value
        //        message.Subject = "Enquiry from your website";
        //        message.Body = string.Format(body, "Gardenwaala", "support@gardenwaala.com");
        //        message.IsBodyHtml = true;

        //        using (var smtp = new SmtpUserPanel())
        //        {
        //            smtp.Host = "webmail.gardenwaala.com";
        //            NetworkCredential NetworkCred = new NetworkCredential("support@gardenwaala.com", "wp#P1r77");
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = NetworkCred;
        //            smtp.Port = 587;
        //            smtp.Send(message);
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}


        ////Send sms 
        //public static bool SendSMS(string name, string contact, string ownerContact)
        //{
        //    HttpUserPanel smsUserPanel = new HttpUserPanel();
        //    smsUserPanel.BaseAddress = new Uri("https://smsleads.in/pushsms.php");

        //    string msgUserPanel = "Thank you for contacting, Gardenwaala Team will get back to you soon.";

        //    string msgOwner = "There is a enquiry from your website, by " + name + " Contact No:" + contact;

        //    string uriDetailsUserPanel = string.Format("?username=amit1223&password=122389&sender=PNMORE&numbers={0}&message={1}", contact, msgUserPanel);

        //    string uriDetailsOwner = string.Format("?username=amit1223&password=122389&sender=PNMORE&numbers={0}&message={1}", ownerContact, msgOwner);

        //    var responseTaskUserPanel = smsUserPanel.GetAsync(uriDetailsUserPanel);
        //    responseTaskUserPanel.Wait();

        //    var responseTaskOwner = smsUserPanel.GetAsync(uriDetailsOwner);
        //    responseTaskOwner.Wait();
        //    var resultUserPanel = responseTaskUserPanel.Result;
        //    var resultOwner = responseTaskOwner.Result;
        //    if (resultUserPanel.IsSuccessStatusCode && resultOwner.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        ////send OTP
        //public static bool SendOTP(string contact, string otp)
        //{
        //    HttpUserPanel smsUserPanel = new HttpUserPanel();
        //    smsUserPanel.BaseAddress = new Uri("https://smsleads.in/pushsms.php");

        //    string msg = "Your OTP for verification is " + otp;


        //    string uriDetailsUserPanel = string.Format("?username=amit1223&password=122389&sender=PNMORE&numbers={0}&message={1}", contact, msg);


        //    var responseTaskUserPanel = smsUserPanel.GetAsync(uriDetailsUserPanel);
        //    responseTaskUserPanel.Wait();

        //    var resultUserPanel = responseTaskUserPanel.Result;
        //    if (resultUserPanel.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        //public static bool SendFeedbackSMS(string contact)
        //{
        //    HttpUserPanel smsUserPanel = new HttpUserPanel();
        //    smsUserPanel.BaseAddress = new Uri("https://smsleads.in/pushsms.php");

        //    string msg = "Thank you for your valuable feedback with our web link";


        //    string uriDetailsUserPanel = string.Format("?username=amit1223&password=122389&sender=PNMORE&numbers={0}&message={1}", contact, msg);


        //    var responseTaskUserPanel = smsUserPanel.GetAsync(uriDetailsUserPanel);
        //    responseTaskUserPanel.Wait();

        //    var resultUserPanel = responseTaskUserPanel.Result;
        //    if (resultUserPanel.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }
}