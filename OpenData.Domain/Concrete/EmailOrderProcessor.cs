using System.Net;
using System.Net.Mail;
using System.Text;
using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Concrete
{

    public class EmailSettings
    {
        public string MailToAddress = "";
        public string MailFromAddress = "leshukovva@gov35.ru";
        public bool UseSsl = true;
        public string Username = "leshukovva@gov35.ru";
        public string Password = "Pk956vb";
        public string ServerName = "gexc1.gov35.ru";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";
    }
    
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings) 
        {
            emailSettings = settings;
        }
        public void ProcessOrder(ShippingDetails shippingInfo, string OperatorEmail) 
        {
            using (var smtpClient = new SmtpClient()) {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username,emailSettings.Password);
                if (emailSettings.WriteAsFile) {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder().AppendLine(string.Format("{0} {1} {2} Сообщает:", shippingInfo.Surname, shippingInfo.Name, shippingInfo.Patronimic));
                body.AppendLine(string.Format("В Вашем наборе, имеющим идентификационный номер {0} в строке {1} обнаружена следующая проблема: {2}", shippingInfo.ODID, shippingInfo.RowNum, shippingInfo.Problem));
                body.AppendLine(string.Format("Дополнительно пользователь сообщает: {0}", shippingInfo.Body));
                body.AppendLine(string.Format("С ним можно связаться по электронной почте: {0}, или по телефону:{1}", shippingInfo.Email,shippingInfo.Phone));
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    OperatorEmail, // To
                    "Получено сообщение об ошибке в наборе открытых данных", // Subject
                    body.ToString()); // Body
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.Unicode;
                }
                smtpClient.Send(mailMessage);
            }   
        }
    }
}
