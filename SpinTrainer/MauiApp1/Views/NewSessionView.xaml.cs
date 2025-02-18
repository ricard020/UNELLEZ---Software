using System.Timers;
using SpinningTrainer.ViewModel;
using Timer = System.Timers.Timer;

namespace SpinningTrainer.Views;

public partial class NewSessionView : ContentPage
{
    private const int MinValue = 0;
    private const int MaxValue = 120;
    private int _currentValue = 0;
    private SessionViewModel _sessionViewModel;
    private Timer _repeatTimer;
    private Action _repeatAction;

    public NewSessionView()
    {
        InitializeComponent();
        _repeatTimer = new Timer(50); // Velocidad de incremento/decremento continuo
        _repeatTimer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(() => _repeatAction?.Invoke());
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        _sessionViewModel = (SessionViewModel)this.BindingContext;
        _currentValue = _sessionViewModel.Duracion;
    }

    private void StartIncrement(object sender, EventArgs e)
    {
        _repeatAction = IncrementValue;
        _repeatTimer.Start();
        IncrementValue();
    }

    private void StartDecrement(object sender, EventArgs e)
    {
        _repeatAction = DecrementValue;
        _repeatTimer.Start();
        DecrementValue();
    }

    private void StopRepeatAction(object sender, EventArgs e)
    {
        _repeatTimer.Stop();
    }

    private void IncrementValue()
    {
        if (_currentValue < MaxValue)
        {
            _currentValue++;
            _sessionViewModel.Duracion = _currentValue;
        }
    }

    private void DecrementValue()
    {
        if (_currentValue > MinValue)
        {
            _currentValue--;
            _sessionViewModel.Duracion = _currentValue;
        }
    }
}

