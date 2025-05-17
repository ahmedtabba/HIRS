using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using TripleA.Utilities;

namespace BoulevardManagement.Utilities
{
    public enum MailTemplates
    {
        [Description("User Added Successfully")]
        UserAdded,
    }
    public class EmailUtil
    {
        public static void SendMail(MailTemplates template, List<string> to, Dictionary<string, string> keys)
        {
            try
            {


                MailMessage mgs = new MailMessage();
                //mgs.From=new MailAddress(Configurations.AdminMail);
                to.ForEach(z => mgs.To.Add(z));
                var main = "";
                using (StreamReader sr = new StreamReader(Configurations.HtmlTemplatePath + "MainTemplate.html"))
                {
                    // Read the stream to a string, and write the string to the console.
                    main = sr.ReadToEnd();
                }
                var body = "";
                using (StreamReader sr = new StreamReader(Configurations.HtmlTemplatePath + template.ToString() + ".html"))
                {
                    // Read the stream to a string, and write the string to the console.
                    body = sr.ReadToEnd();
                }
                main = main.Replace("@Content", body);
                keys.Keys.ToList().ForEach(x =>
                {

                    main = main.Replace(x, keys[x]);
                });
                mgs.Subject = template.GetEnumDescription();
                mgs.Body = main;

                SendMail(mgs);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("{0}{1}", "Sending Email Is Failed! --", ex.Message));
            }
        }


        public static void SendMail(MailMessage msg)
        {
            try
            {
                //embeded image in Email


                string textMail = Configurations.TestMail;

                if (!String.IsNullOrEmpty(textMail))
                {
                    //var view = AlternateView.CreateAlternateViewFromString(textMail, null, MediaTypeNames.Text.Html);
                    //var links = new List<LinkedResource>();
                    //links.Add(new LinkedResource(Path.Combine(MailImagesPath, "logo.png"), MediaTypeNames.Image.Jpeg) { ContentId = "LogoImage" });
                    //links.Add(new LinkedResource(Path.Combine(MailImagesPath, "enjaz-logo.png"), MediaTypeNames.Image.Jpeg) { ContentId = "enjaz-logo" });
                    //links.ForEach(link => { view.LinkedResources.Add(link); });
                    string RealEmails = string.Format("--- Should be sent to: (TO:{0})",
                        string.Join(",", msg.To.Select(p => p.Address).ToArray()),
                        string.Join(",", msg.CC.Select(p => p.Address).ToArray())
                        );
                    msg.Subject = msg.Subject + RealEmails;
                    //msg.To.Clear();
                    //msg.CC.Clear();
                    //msg.Bcc.Clear();
                   // msg.To.Add(msg.To);
                }



                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1256");

                var smtp = new SmtpClient();
                //{
                //    Host = Configurations.MailHost,
                //    Port =Convert.ToInt32( Configurations.MailPort),
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new System.Net.NetworkCredential(Configurations.AdminMail, Configurations.AdminPassword)
                //};

                smtp.Send(msg);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public static void SendErrorMail(string subject, string FilePath)
        {
            try
            {


                MailMessage mgs = new MailMessage();
                mgs.From = new MailAddress("FromMail@gmail.com");
                mgs.To.Add(Configurations.TestMail);
                var main = "";
                mgs.Subject = subject;
                var body = "";

                mgs.Attachments.Add(new Attachment(FilePath));

                main = main.Replace("@Content", body);


                mgs.Body = main;

                SendMail(mgs);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("{0}{1}", "Sending Email Is Failed! --", ex.Message));
            }
        }

    }
}
