using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;

namespace UTILITIES.ToastMessagesUtility
{
    public class ToastMessagesUtility : IToastMessagesUtility
    {
        public async Task ShowMessage(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ToastDuration duration = ToastDuration.Long;
            var toast = Toast.Make(message, duration, 14);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
