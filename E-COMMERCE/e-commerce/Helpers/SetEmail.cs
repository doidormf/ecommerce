using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using e_commerce.Properties;
using VipWebUtils.Helpers.Exceptions;
using VipWebUtils.Helpers.Extensoes;
using VipWebUtils.Helpers.Security;

namespace e_commerce.Helpers
{
    public class SetEmail
    {
        protected string status = string.Empty;
        private bool chaveEmail = true;
        private int contator = 0;

        public string SendEmail(string email, string assunto, string menssagem, string emailEnvio, string senhaEnvio, bool chave = true)
        {
            return setEmail(email, assunto, menssagem, emailEnvio, senhaEnvio, chave);
        }

        private string setEmail(string email, string assunto, string menssagem, string emailEnvio, string senhaEnvio, bool chave = true)
        {
            while (chaveEmail == true)
            {
                try
                {

                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    client.Host = Settings.Default.SmtpServer;
                    client.Port = 587;

                    string senha = Crypt.Decrypter(senhaEnvio);

                    NetworkCredential credentials =
                     new NetworkCredential(emailEnvio, senha);
                    client.UseDefaultCredentials = true;
                    client.Credentials = credentials;

                    MailMessage msg = new MailMessage();
                    if (chave == false)
                    {
                        msg.From = new MailAddress(email);//remetente
                        msg.To.Add(new MailAddress(emailEnvio));//destinatário
                        msg.ReplyToList.Add(email);
                    }
                    else
                    {
                        msg.From = new MailAddress(emailEnvio);//remetente
                        msg.To.Add(new MailAddress(email));//destinatário
                    }

                    msg.Priority = MailPriority.Normal;
                    msg.Subject = assunto;
                    msg.IsBodyHtml = true;
                    msg.Body = menssagem;
                    client.Send(msg);
                    status = "E-mail enviado com sucesso!";
                    chaveEmail = false;
                }
                catch (Exception ex)
                {
                  
                    StackTrace exe = new StackTrace(ex, true);
                    CustomException ep = new CustomException(ex, exe, "");
                    ep.Save(AppDomain.CurrentDomain.BaseDirectory + "/Logs/LogMail.log");
                    contator++;
                    if (contator >= 10)
                    {
                        status = "No momento seu e-mail não poder ser enviado! Tente novamente mais tarde.";
                        chaveEmail = false;
                    }
                }
            }
            return status;
        }
    }
}