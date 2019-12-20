using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SPUtility
{
    public static class EmailSender
    {
        public static string url { get; set; }
        public static string Sub { get; set; }

        public static void Send(string email,string text )
        {            
            string smtp = ConfigurationManager.AppSettings["smtp"];
            string ownerEemail = ConfigurationManager.AppSettings["ownerEemail"];
            string password = ConfigurationManager.AppSettings["password"];
            string subject = ConfigurationManager.AppSettings["subject"];
            string Port = ConfigurationManager.AppSettings["Port"];
           
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtp);
            mail.From = new MailAddress(email);
            mail.To.Add(email);
            mail.Subject = subject+ Sub;
            mail.Body = text;
            mail.IsBodyHtml = true;
            SmtpServer.Port = int.Parse(Port);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ownerEemail, password);
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch 
            {                
                throw new Exception("อีเมลไม่ถูกต้อง");
            }              
        }
    }
}
