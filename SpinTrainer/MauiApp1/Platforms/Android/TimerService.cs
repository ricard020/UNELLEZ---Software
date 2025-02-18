using Android.App;
using Android.Content;
using Android.OS;
using System.Timers;
using Timer = System.Timers.Timer;

[Service]
public class TimerService : Service
{
    private Timer _timer;

    public TimerService()
    {
        _timer = new Timer(100);
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        _timer.Start();
        return StartCommandResult.Sticky;
    }

    public void StartTimer()
    {
        _timer.Start();
    }

    public void StopTimer()
    {
        _timer.Stop(); 
        _timer.Elapsed -= OnTimerElapsed;
    }

    public bool GetEnabled()
    {
        return _timer.Enabled;
    }

    public void SetEnabled(bool enable)
    {
        _timer.Enabled = enable;
    }

    // Método público para que el ViewModel pueda suscribirse al evento
    public void SetTimerElapsedEvent(ElapsedEventHandler eventHandler)
    {
        _timer.Elapsed += eventHandler;  // Asigna el manejador desde el ViewModel
    }

    public override void OnDestroy()
    {
        _timer.Elapsed -= OnTimerElapsed;  // Desvincula el evento
        _timer.Stop();
        base.OnDestroy();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Aquí va tu lógica del Timer
    }

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }
}
