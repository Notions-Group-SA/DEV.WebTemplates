using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Notions.Core.Utils.Common
{
    public class NgEmail
    {
        NgEmailModel Modelo = new();

        #region Metodos Publicos

        public void send(NgEmailModel Model)
        {
            Modelo = Model;
            pSend();
        }

        public void Send(NgEmailModel Model, Char separador)
        {
            try
            {
                Modelo = Model;

                Char[] splitter = { separador };
                string[] arEmails;

                arEmails = Modelo.To.Trim().Split(splitter);
                for (int i = 0; i < arEmails.Length; i++)
                {
                    Modelo.To = arEmails[i];
                    pSend();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Metodos Privados

        private void pSend()
        {
            try
            {
                MailMessage MyEmail = new MailMessage();
                MyEmail.From = new MailAddress(Modelo.FromEmail, Modelo.FromName);
                MyEmail.To.Add(Modelo.To);
                MyEmail.Subject = Modelo.Subject;
                MyEmail.Body = Modelo.Body;
                MyEmail.IsBodyHtml = true;
                if (Modelo.CC != null)
                {
                    foreach (MailAddress copia in Modelo.CC)
                    {
                        MyEmail.CC.Add(copia);
                    }
                }


                if (Modelo.Attach1 != "")
                {
                    Attachment _attach = new Attachment(Modelo.Attach1);
                    MyEmail.Attachments.Add(_attach);
                }
                if (Modelo.Attach2 != "")
                {
                    Attachment _attach2 = new Attachment(Modelo.Attach2);
                    MyEmail.Attachments.Add(_attach2);
                }
                SmtpClient smtpCliente = new SmtpClient();

                smtpCliente.Host = Modelo.SMTP;
                smtpCliente.Port = Modelo.Port;
                smtpCliente.Credentials = new System.Net.NetworkCredential(Modelo.User, Modelo.Password);

                smtpCliente.Send(MyEmail);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

    public class NgEmailModel
    {
        public string FromName {  get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public MailAddressCollection CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Attach1 {  get; set; }
        public string Attach2 { get; set; }

    }
}
