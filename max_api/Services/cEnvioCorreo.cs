using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System;

namespace max_api.Services
{
    public class cEnvioCorreo : IEnvioCorreo
    {
        public cEnvioCorreo()
        {
        }
        public bool EnvioCorreo(string Smtp, int Puerto, string EmailRemiten, string pwd, int ssl, List<string> lista, string Titulo, string MensajeHTLM, string DirEnviarImg, string GuidId)
        {
            bool ok = false;
            try
            {
                MailMessage correo = new MailMessage() { From = new MailAddress(EmailRemiten), SubjectEncoding = Encoding.UTF8, BodyEncoding = Encoding.UTF8 };
                foreach (string ele in lista)
                {
                    correo.To.Add(ele);
                }
                correo.Subject = Titulo;
                correo.IsBodyHtml = true;
                if (DirEnviarImg != "")
                {
                    LinkedResource inline = new LinkedResource(DirEnviarImg, MediaTypeNames.Image.Jpeg);
                    inline.ContentId = GuidId;
                    inline.TransferEncoding = TransferEncoding.QuotedPrintable;
                    inline.ContentType.Name = GuidId;
                    inline.ContentLink = new Uri("cid:" + GuidId);

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                              MensajeHTLM,
                                              Encoding.UTF8,
                                              MediaTypeNames.Text.Html);

                    htmlView.LinkedResources.Add(inline);
                    correo.AlternateViews.Add(htmlView);



                }
                else
                {
                    correo.Body = MensajeHTLM;
                }

                if (pwd.Trim() == "")
                {
                    SmtpClient smtpCli = new SmtpClient() { Host = Smtp, Port = Puerto, EnableSsl = ssl==1?true:false };
                    smtpCli.Send(correo);
                }
                else
                {
                    SmtpClient smtpCli = new SmtpClient() { Host = Smtp, Port = Puerto, EnableSsl = ssl == 1 ? true : false, UseDefaultCredentials = false };
                    smtpCli.Credentials = new NetworkCredential(EmailRemiten, pwd);
                    smtpCli.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpCli.Send(correo);
                }

                ok = true;

            }
            catch (Exception ex)
            {
                #region LOG_ERROR
                //guardar la excepcion en un registro en mongo
               
                #endregion
                ok = false;
            }
            return ok;
        }

    }
}
