using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Gardenwaala.Areas.Client.Models
{
    public class SendEmail
    {
        public static string EmailEnqBody(string UserName, string Message)
        {
            return string.Format("<html><head></head><body><table style='background:#ffffff;max-width:520px' align='center' width='520' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff'> <tbody> <tr> <td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td><td width='480'> <table width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td style='background:#eeeeee' height='20' bgcolor='#eeeeee'></td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='49'></td></tr><tr> <td style='color:#4285f4;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:32px;font-weight:300;line-height:46px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Hi {0},</td></tr><tr> <td height='20'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>{1}<br /></b></td></tr><tr> <td height='30'></td></tr></tbody> </table> </td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='32'></td></tr><tr> <td align='center'> <img src='https://gardenwaala.com/Areas/Client/Assets/images/logo.png' alt='Gardenwaala' width='200' border='0'> </td></tr><tr> <td height='15'></td></tr></tbody> </table> </td></tr><tr> <td> <table align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='27'></td></tr><tr> <td height='25'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Thanks & Regards,</td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Gardenwaala Team</td></tr><tr> <td height='36'></td></tr><tr> <td height='31'></td></tr></tbody> </table> </td></tr><tr> <td style='background:#eeeeee' height='18' bgcolor='#eeeeee'></td></tr></tbody> </table> </td><td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td></tr></tbody></table></body></html>", UserName, Message);
        }

        public static bool EmailBody(string name, string email, string msg, string sub)
        {
            try
            {
                var body = "<html><head></head><body><table style='background:#ffffff;max-width:520px' align='center' width='520' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff'> <tbody> <tr> <td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td><td width='480'> <table width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td style='background:#eeeeee' height='20' bgcolor='#eeeeee'></td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='49'></td></tr><tr> <td style='color:#4285f4;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:32px;font-weight:300;line-height:46px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Hi " + name + ",</td></tr><tr> <td height='20'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>" + msg + "</td></tr><tr> <td height='30'></td></tr></tbody> </table> </td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='32'></td></tr><tr> <td align='center'> <img src='https://gardenwaala.com/Areas/Client/Assets/images/logo.png' alt='Gardenwaala' width='200' border='0'> </td></tr><tr> <td height='15'></td></tr></tbody> </table> </td></tr><tr> <td> <table align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='27'></td></tr><tr> <td height='25'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Thanks & Regards,</td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Gardenwaala Team</td></tr><tr> <td height='35'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Keep in touch: <a href='#' target='_blank'>Instagram</a>, <a href='https://www.facebook.com/gardenwaala' target='_blank'>Facebook</a>, <a href='https://goo.gl/maps/559xQAtka4YLjFEj8' target='_blank'>Google+</a></td></tr><tr> <td height='27'></td></tr><tr> <td height='36'></td></tr><tr> <td height='31'></td></tr></tbody> </table> </td></tr><tr> <td style='background:#eeeeee' height='18' bgcolor='#eeeeee'></td></tr></tbody> </table> </td><td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td></tr></tbody></table></body></html>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.From = new MailAddress("support@gardenwaala.com", "Gardenwaala Support");  // replace with valid value
                message.Subject = sub;
                message.Body = string.Format(body, "Gardenwaala", "support@gardenwaala.com");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {

                    smtp.Host = "smtp.gmail.com";
                    NetworkCredential NetworkCred = new NetworkCredential("support@gardenwaala.com", "supgar123!@#");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EmailOrderBody(string name, string email, string msg, string sub)
        {
            try
            {
                var body = "<html><head></head>" +
                    "<body>" +
                    "<table style='background:#ffffff;max-width:700px !important' align='center' width='800' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff'> " +
                    "<tbody>" +
                    "<tr>" +
                    "<td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td>" +
                    "<td width='700'>" +
                    "<table width='100%' cellspacing='0' cellpadding='0' border='0'>" +
                    "<tbody>" +
                    "   <tr>" +
                            "<td style='background:#eeeeee' height='20' bgcolor='#eeeeee'></td>" +
                        "</tr>" +
                        "<tr> <td> <table style='border-bottom:1px solid #eeeeee' align='left' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='32'></td></tr><tr> <td align='center'> <img src='https://gardenwaala.com/Areas/Client/Assets/images/logo.png' alt='Gardenwaala' width='200' border='0'> </td></tr><tr> <td height='15'></td></tr></tbody> </table> </td></tr>" +
                        "<tr>" +
                        "<td>" +
                        "<table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'>" +
                        " <tbody>" +
                        " <tr>" +
                        " <td height='20'></td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:15px;line-height:46px;margin:0;padding:0 25px 0 25px;text-align:left' align='center'>Hi " + name + ",</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td height='20'></td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:15px;line-height:46px;margin:0;padding:0 25px 0 25px;text-align:left' align='center'>Your order is placed successfully and it's on the way...</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>" +
                        "<table style='background:#ffffff;max-width:700px !important' align='center' width='100%' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff'><tbody>" + msg + "</tbody></thead></td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td height='30'></td>" +
                        "</tr>" +
                        "</tbody>" +
                        "</table>" +
                        "</td></tr><tr> <td> <table align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='27'></td></tr><tr> <td height='25'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Thanks & Regards,</td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Gardenwaala Team</td></tr><tr> <td height='35'></td></tr><tr> <td style='font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Keep in touch: <a href='#' target='_blank'>Instagram</a>, <a href='https://www.facebook.com/gardenwaala' target='_blank'>Facebook</a>, <a href='https://goo.gl/maps/559xQAtka4YLjFEj8' target='_blank'>Google+</a></td></tr><tr> <td height='27'></td></tr><tr> <td height='36'></td></tr><tr> <td height='31'></td></tr></tbody> </table> </td></tr><tr> <td style='background:#eeeeee' height='18' bgcolor='#eeeeee'></td></tr></tbody> </table> </td><td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td></tr></tbody></table></body></html>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.From = new MailAddress("support@gardenwaala.com", "Gardenwaala Support");  // replace with valid value
                message.Subject = sub;
                message.Body = string.Format(body, "Gardenwaala", "support@gardenwaala.com");
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    NetworkCredential NetworkCred = new NetworkCredential("support@gardenwaala.com", "supgar123!@#");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}