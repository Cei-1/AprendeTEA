using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AprendeTEA_19032025.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task EnviarConfirmacionEmailAsync(string email, int idUsuario, string token)
        {
            //string url = $"http://localhost:11399/Login/ConfirmarEmail?id={idUsuario}&token={token}";
            string url = $"http://localhost:5138/Login/ConfirmarEmail?id={idUsuario}&token={token}";
            //string url = $"https://tusitio.com/Login/ConfirmarEmail?id={idUsuario}&token={token}";

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = "Confirmación de correo - Neuro Pro",
                //Body = $@"
                //<img src='~/img/latimente.png' alt='Neuro Pro' style='width:90px; margin-bottom:20px;' />

                //    <h2>Bienvenido a Neuro Pro</h2>
                //    <p>Gracias por registrarte. Para activar tu cuenta, da clic en el siguiente enlace:</p>
                //    <p><a href='{url}' target='_blank'>Confirmar mi correo</a></p>
                //    <p>Si tú no realizaste este registro, puedes ignorar este correo.</p>",
                Body = $@"
                        <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6fb; padding:30px 0; font-family:Arial, Helvetica, sans-serif;'>
                            <tr>
                                <td align='center'>
                                    <table width='600' cellpadding='0' cellspacing='0' style='background:white; border-radius:12px; box-shadow:0 4px 12px rgba(0,0,0,0.08); padding:40px;'>
                                        <tr>
                                            <td align='center'>
                                                <h2 style='color:#729BFF; margin:0; font-size:26px;'>¡Bienvenido a Neuro Pro!</h2>
                                                <p style='color:#444; font-size:15px; margin-top:15px; line-height:1.6;'>
                                                    Gracias por unirte a nuestra comunidad. Antes de comenzar, necesitamos que confirmes tu cuenta.
                                                </p>

                                                <a href='{url}' target='_blank'
                                                    style='display:inline-block; margin-top:25px; padding:14px 28px;
                                                           background:linear-gradient(135deg, #729BFF 0%, #B2A1FF 100%);
                                                           color:white; text-decoration:none; border-radius:8px;
                                                           font-size:16px; font-weight:bold;'>
                                                    Confirmar mi correo
                                                </a>

                                                <p style='color:#888; margin-top:25px; font-size:13px; line-height:1.6;'>
                                                    Si tú no realizaste este registro, simplemente ignora este mensaje.<br>
                                                    Este enlace es válido solo para este proceso de activación.
                                                </p>

                                                <hr style='margin:30px 0; border:none; border-top:1px solid #eaeaea;' />

                                                <p style='color:#999; font-size:12px;'>
                                                    © {DateTime.Now.Year} Neuro Pro — Todos los derechos reservados.
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>",

                IsBodyHtml = true
            };

            mail.To.Add(email);

            using (var smtp = new SmtpClient())
            {
                smtp.Host = _settings.SmtpServer;
                smtp.Port = _settings.SmtpPort;
                smtp.EnableSsl = _settings.EnableSsl;
                smtp.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(mail);
            }
        }
    }

}
