using System.Net.Mail;

namespace UTILITIES.MailUtility
{
    public interface IMailUtility
    {
        /// <summary>
        /// Clase para enviar correo con un código aleatorio de 8 dígitos, en caso de error retorna el número 0 y el mensaje del error.
        /// </summary>
        /// <param name="destination">Correo electrónico de destination</param>
        /// <returns>Devuelve el número enviado y mensaje de error en caso de que haya uno. (Si ocurre algún error el número retornara 0)</returns>
        (string, string) SendMailRecoveryCode(string destination);

        /// <summary>
        /// Envía un correo
        /// </summary>
        /// <param name="destination">Destinatario del correo.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del correo.</param>
        /// <returns>Devuelve resultado si el envió culmino exitosamente y un mensaje de error en caso de que no.</returns>
        public (bool, string) SendMail(string destination, string subject, string body);        
    }
}
