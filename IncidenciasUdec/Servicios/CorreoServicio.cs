using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using IncidenciasUdec.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace IncidenciasUdec.Servicios
{
    public static class CorreoServicio
    {
        public static bool Enviar(Correo correo)
        {
            string _host = "smtp.gmail.com";
            int _puerto = 587;
            string _nombreEmisor = "Incidencias UDEC";
            string _email = "pruebacundiinc@gmail.com";
            string _pass = "ncexzhetedfyfxct";
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_nombreEmisor, _email));
                email.To.Add(MailboxAddress.Parse(correo.Para));
                email.Subject = correo.Asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = correo.Contenido
                };

                var smpt = new SmtpClient();
                smpt.Connect(_host, _puerto, SecureSocketOptions.StartTls);
                smpt.Authenticate(_email, _pass);
                smpt.Send(email);
                smpt.Disconnect(true);

                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
                
            }
        }
    }
}