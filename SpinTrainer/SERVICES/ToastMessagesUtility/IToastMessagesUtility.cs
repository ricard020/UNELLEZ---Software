namespace UTILITIES.ToastMessagesUtility
{
    public interface IToastMessagesUtility
    {
        /// <summary>
        /// Muestra un mensaje Toast de Android.
        /// </summary>
        /// <param name="message">Mensaje.</param>        
        Task ShowMessage(string message);
    }
}
