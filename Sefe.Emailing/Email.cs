using Sefe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Sefe.Extentions;
using Sefe.ApplicationSettings;
using System.ComponentModel;

namespace Sefe.Emailing
{
    /// <summary>
    /// Send e-mail
    /// Single adrese and multiple adrese can be sent. If the "To Address List" value is full, the single address value is not read.
    /// Required config settings:
    /// Smtp_Port
    /// Smtp_Host
    /// Smtp_Username
    /// Smtp_Password
    /// </summary>
    public class Email
    {
        #region Local properties
        private int Port { get; set; }
        private string Host { get; set; }
        private string Password { get; set; }
        private int Timeout { get; set; }
        #endregion Local properties

        #region Public properties
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public List<string> ToAddressList { get; set; }
        public bool IsHtmlBody { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool EnableSsl { get; set; }
        public int UseDefaultCredentials { get; set; }
        /// <summary>
        /// A unique value for the email. The callback terminator can be used to read and update the database. For this reason it is a required parameter.
        /// </summary>
        public string Key { get; set; }
        #endregion Public properties

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">A unique value for the email. The callback terminator can be used to read and update the database. For this reason it is a required parameter.</param>
        public Email(string key)
        {
            this.Key = key;
            Port = Settings.GetAppSetting("Smtp_Port", 0);
            Host = Settings.GetAppSetting("Smtp_Host", string.Empty);
            FromAddress = Settings.GetAppSetting("Smtp_Username", string.Empty);
            Password = Settings.GetAppSetting("Smtp_Password", string.Empty);
            Timeout = Settings.GetAppSetting("Smtp_Timeout", 10000);
            UseDefaultCredentials = Settings.GetAppSetting("Smtp_UseDefaultCredentials", 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">A unique value for the email. The callback terminator can be used to read and update the database. For this reason it is a required parameter.</param>
        /// <param name="Subject"></param>
        /// <param name="ToAddress"></param>
        /// <param name="ToAddressList"></param>
        /// <param name="Body"></param>
        /// <param name="IsHtmlBody"></param>
        /// <param name="EnableSsl"></param>
        /// <param name="UseDefaultCredentials"></param>
        public Email(string Key, string Subject, string ToAddress, List<string> ToAddressList, string Body, bool IsHtmlBody = true, bool EnableSsl = true, int UseDefaultCredentials = 1)
        {
            Port = Settings.GetAppSetting("Smtp_Port", 0);
            Host = Settings.GetAppSetting("Smtp_Host", string.Empty);
            FromAddress = Settings.GetAppSetting("Smtp_Username", string.Empty);
            Password = Settings.GetAppSetting("Smtp_Password", string.Empty);
            Timeout = Settings.GetAppSetting("Smtp_Timeout", 10000);
            UseDefaultCredentials = Settings.GetAppSetting("Smtp_UseDefaultCredentials", 1);

            this.Key = Key;
            this.Subject = Subject;
            this.ToAddress = ToAddress;
            this.ToAddressList = ToAddressList;
            this.Body = Body;
            this.IsHtmlBody = IsHtmlBody;
            this.EnableSsl = EnableSsl;
            this.UseDefaultCredentials = UseDefaultCredentials;
        }
        #endregion Constructor

        #region Delegates
        public delegate void OnAsyncSent(bool isSucces, string message, string key);
        public event OnAsyncSent OnSent;
        #endregion Delegates

        private ProcessResult GetClient()
        {
            ProcessResult result = new ProcessResult();
            #region Validations
            if (Port == 0)
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = "'Smtp_Port' is not found in config file";
                return result;
            }
            else if (string.IsNullOrEmpty(Host))
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = "'Smtp_Host' is not found in config file";
                return result;
            }
            else if (string.IsNullOrEmpty(FromAddress))
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = "'Smtp_Username' is not found in config file";
                return result;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = "'Smtp_Password' is not found in config file";
                return result;
            }
            #endregion Validations

            // set smtp client variables
            SmtpClient client = new SmtpClient();
            client.Port = this.Port;
            client.Host = this.Host;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = UseDefaultCredentials == 1;
            client.Credentials = new System.Net.NetworkCredential(FromAddress, Password);
            client.EnableSsl = EnableSsl;
            client.Timeout = Timeout;
            result.ReturnObject = client;

            return result;
        }

        public ProcessResult Send()
        {
            ProcessResult result = GetClient();
            if (!result.IsSuccess())
            {
                return result;
            }
            SmtpClient client = (SmtpClient)result.ReturnObject;

            if (this.ToAddressList != null)
            {
                #region send multiple email
                foreach (var item in ToAddressList)
                {
                    try
                    {
                        MailMessage mail = new MailMessage(this.FromAddress, item);
                        mail.BodyEncoding = UTF8Encoding.UTF8;
                        mail.IsBodyHtml = this.IsHtmlBody;
                        mail.Subject = this.Subject;
                        mail.Body = this.Body;
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        result.ErrorList.Add(string.Format("\n\rAdres:{0}\n\r{1}", item, ex.GetExceptionMessage()));
                        continue;
                    }
                }
                if (result.ErrorList.Count > 0)
                {
                    result.ResultType = ProcessResultTypes.CompletedWithError;
                    StringBuilder sb = new StringBuilder("Some emails couldn't send. Check details.");
                    sb.AppendLine();
                    sb.AppendLine(string.Join(",", result.ErrorList));
                    result.ErrorMessage = sb.ToString();
                }
                #endregion send multiple email
            }
            else
            {
                #region send single email
                MailMessage mail = new MailMessage(this.FromAddress, this.ToAddress);
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.IsBodyHtml = this.IsHtmlBody;
                mail.Subject = this.Subject;
                mail.Body = this.Body;
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    result.ResultType = ProcessResultTypes.SystemError;
                    result.SystemError = ex;
                }
                #endregion send single email
            }

            return result;
        }

        public void SendAsync()
        {
            ProcessResult result = GetClient();
            if (result.IsSuccess())
            {
                SmtpClient client = (SmtpClient)result.ReturnObject;
                MailAddress fromAddress = new MailAddress(this.FromAddress);
                MailMessage mail = new MailMessage();
                mail.From = fromAddress;
                if (this.ToAddressList != null && this.ToAddressList.Count > 0)
                {
                    foreach (var item in this.ToAddressList)
                    {
                        mail.To.Add(new MailAddress(item));
                    }
                }
                else
                {
                    mail.To.Add(new MailAddress(this.ToAddress));
                }
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.SubjectEncoding = UTF8Encoding.UTF8;
                mail.IsBodyHtml = this.IsHtmlBody;
                mail.Subject = this.Subject;
                mail.Body = this.Body;
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                try
                {
                    client.SendAsync(mail, mail);
                }
                catch
                {

                }
            }
            else
            {
                OnSent(false, result.ErrorMessage, Key);
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the message we sent
            MailMessage msg = (MailMessage)e.UserState;

            if (e.Cancelled)
            {
                // Cancelled
                OnSent(false, "Operation cancelled", Key);
            }
            if (e.Error != null)
            {
                // Error
                OnSent(false, e.Error.Message, Key);
            }
            else
            {
                // Success
                OnSent(true, null, Key);
            }

            // finally dispose of the message
            if (msg != null)
                msg.Dispose();
        }
    }
}
