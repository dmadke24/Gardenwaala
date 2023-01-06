using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace GardenWaalaAPI.Models
{
    public class SendEmail
    {
        public static bool EmailBody(string name, string email, string msg, string sub)
        {
            try
            {
                var body = "<table style='background:#ffffff;max-width:520px' align='center' width='520' cellspacing='0' cellpadding='0' border='0' bgcolor='#ffffff'> <tbody> <tr> <td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td><td width='480'> <table width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td style='background:#eeeeee' height='20' bgcolor='#eeeeee'></td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='49'></td></tr><tr> <td style='color:#4285f4;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:32px;font-weight:300;line-height:46px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Hi " + name + ",</td></tr><tr> <td height='20'></td></tr><tr> <td style='color:#757575;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>" + msg + "</td></tr><tr> <td height='30'></td></tr></tbody> </table> </td></tr><tr> <td> <table style='border-bottom:1px solid #eeeeee' align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='32'></td></tr><tr> <td align='center'> <img src='https://gardenwaala.com/Areas/Client/Assets/images/logo.png' alt='Gardenwaala' width='200' border='0'> </td></tr><tr> <td height='15'></td></tr></tbody> </table> </td></tr><tr> <td> <table align='center' width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td height='27'></td></tr><tr> <td height='25'></td></tr><tr> <td style='color:#757575;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Thanks & Regards,</td></tr><tr> <td style='color:#757575;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Gardenwaala Team</td></tr><tr> <td height='35'></td></tr><tr> <td style='color:#757575;font-family:&quot;Roboto&quot;,OpenSans,&quot;Open Sans&quot;,Arial,sans-serif;font-size:17px;font-weight:300;line-height:24px;margin:0;padding:0 25px 0 25px;text-align:center' align='center'>Keep in touch: <a href='#'>Twitter</a>, <a href='#'>Facebook</a>, <a href='#'>Google+</a></td></tr><tr> <td height='27'></td></tr><tr> <td> <table width='100%' cellspacing='0' cellpadding='0' border='0'> <tbody> <tr> <td colspan='3' style='text-align:center;font-family:'Roboto',OpenSans,'Open Sans',Arial,sans-serif;font-size:17px;line-height:24px;font-weight:300;color:#757575' align='center'>Available On.</td></tr><tr> <td colspan='3' style='height:10px'></td></tr><tr> <td style='text-align:right;line-height:10px' width='229'> <a><section><img src='PlayStore.png' alt='Google Play' style='height:48px;width:162px;outline:none;display:block' align='right' width='162' height='48'></section></a> </td><td style='width:21px;height:48px' width='21' height='48'> </td><td style='text-align:left;line-height:10px' width='230'> <a href=''><section><img src='AppleStore.png' alt='App Store' style='width:162;height:48px;outline:none;display:block' align='left' width='162' height='48'></section></a> </td></tr></tbody> </table> </td></tr><tr> <td height='36'></td></tr><tr> <td height='31'></td></tr></tbody> </table> </td></tr><tr> <td style='background:#eeeeee' height='18' bgcolor='#eeeeee'></td></tr></tbody> </table> </td><td style='background:#eeeeee' width='20' bgcolor='#eeeeee'></td></tr></tbody></table>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.From = new MailAddress("info@gardenwaala.com", "Gardenwaala Support");  // replace with valid value
                message.Subject = sub;
                message.Body = string.Format(body, "Gardenwaala", "info@gardenwaala.com");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {

                    smtp.Host = "webmail.gardenwaala.com";
                    NetworkCredential NetworkCred = new NetworkCredential("info@gardenwaala.com", "pT_81kd2");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 25;
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