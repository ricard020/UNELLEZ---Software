using System.Net.Mail;
using System.Net;

namespace UTILITIES.MailUtility
{
    public class MailUtility : IMailUtility
    {
        private SmtpClient _smtpClient = new SmtpClient("smtp-mail.outlook.com", 587);

        /// <summary>
        /// Método constructor de la clase.
        /// </summary>
        public MailUtility()
        {
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential("spincoach@hotmail.com", "Spin2024");
        }
        
        public (string, string) SendMailRecoveryCode(string destination)
        {
            string randomNumber = GenerateRandomNumber();

            var (successfulMailSending, errorMessage) = SendMail(destination, "Código de Recuperación de la app SpinCoach", "Su código de recuperación es: " + randomNumber);

            if (successfulMailSending)
            {
                Console.WriteLine("Correo enviado correctamente.");
                return (randomNumber, "");
            }
            else
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + errorMessage);
                return ("0", errorMessage);
            }
        }

        public (bool, string) SendMail(string destination, string subject, string body)
        {
            // Creación del mensaje
            MailMessage message = new MailMessage();
            message.From = new MailAddress("spincoach@hotmail.com");
            message.To.Add(destination);
            message.Subject = subject;
            message.Body = body;

            try
            {
                _smtpClient.Send(message);
                Console.WriteLine("Correo enviado correctamente.");
                return (true, "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Método para generar un número aleatorio.
        /// </summary>
        /// <returns>Devuelve un número aleatorio de 8 dígitos convertido a String</returns>
        private static string GenerateRandomNumber()
        {
            Random random = new Random();

            int minValue = 10000000;
            int maxValue = 99999999;

            return random.Next(minValue, maxValue + 1).ToString();
        }
    }
}
