using SERVICES.NavigationServices;
using SpinTrainer.Resources.Charts;
using SpinTrainer.ViewModels;
using System.Diagnostics;

namespace SpinningTrainer.Views;

public partial class RemoteControlView : ContentPage
{
    private bool _reconnected = false;

    private TimeSpan _reconnectionActualExerciseTimeSpan = TimeSpan.Zero;

    private GraphDrawable _drawable;
    private RemoteControlViewModel _remoteControlViewModel;
    private List<DataPoint> _dataPoints;

    private int _currentSegmentIndex = 0;

    private Stopwatch _stopwatch = new Stopwatch();

    public TaskCompletionSource<bool> OnModalClosedTask;
    private readonly INavigationServices _navigationServices;

    public RemoteControlView(INavigationServices navigationServices)
	{
		InitializeComponent();
        _navigationServices = navigationServices;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        var viewModel = (RemoteControlViewModel)this.BindingContext;
        _remoteControlViewModel = viewModel;
        viewModel.StartGraphAnimation += () => MainThread.BeginInvokeOnMainThread(() => StartAnimation());
        viewModel.NextExercise += () => MainThread.BeginInvokeOnMainThread(() => NextExercise());
        viewModel.PreviousExercise += () => MainThread.BeginInvokeOnMainThread(() => PreviousExercise());
        viewModel.PauseOrPlayGraphAnimation += () =>
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_stopwatch.IsRunning)
                {
                    _stopwatch.Stop();
                }
                else
                {
                    _stopwatch.Start();
                }
            });

        viewModel.InsertRestingExercise += () => MainThread.BeginInvokeOnMainThread(() => InsertRestingExercise());
        viewModel.ModifyExerciseIntensity += newIntensity => MainThread.BeginInvokeOnMainThread(() => UpdateTargetIntensity(newIntensity));

        var (dataPoints, graphProgress, currentSegmentIndex, startDatetime) = viewModel.LoadExercisesInChart();

        _dataPoints = dataPoints;
        _currentSegmentIndex = currentSegmentIndex;

        if (graphProgress > 0)
        {
            _drawable = new GraphDrawable(_dataPoints);
            _drawable.Progress = graphProgress;
            _drawable.CurrentSegmentIndex = currentSegmentIndex;
            _reconnected = true;
            _reconnectionActualExerciseTimeSpan = DateTime.Now - startDatetime;
        }
        else
            _drawable = new GraphDrawable(_dataPoints);        

        graphicsView.Drawable = _drawable;
    }

    private void NextExercise()
    {
        if (_drawable.CurrentSegmentIndex < _dataPoints.Count - 1)
        {
            if ((_currentSegmentIndex + 1) % 2 != 0)
                _currentSegmentIndex += 2;
            else
                _currentSegmentIndex++;

            graphicsView.Invalidate(); // Refresca la vista gráfica.
        }
    }

    private void PreviousExercise()
    {
        if (_drawable.CurrentSegmentIndex > 0)
        {
            if ((_currentSegmentIndex + 1) % 2 != 0)
                _currentSegmentIndex -= 2;
            else
                _currentSegmentIndex -= 3;
            graphicsView.Invalidate(); // Refresca la vista gráfica.
        }
    }

    private void InsertRestingExercise()
    {
        var viewmodel = (RemoteControlViewModel)this.BindingContext;

        var exercise = viewmodel.GetRestingExercise();

        int intensityPercentageFrom = (int)Math.Round((0.3 * (double)exercise.RPMFin) + (0.7 * (double)exercise.ResistancePercentage) + 20);

        InsertDataPoint(new DataPoint(_dataPoints[_currentSegmentIndex + 2].Time + ((double)exercise.DurationMin / 2), intensityPercentageFrom, (double)exercise.DurationMin / 2, Microsoft.Maui.Graphics.Color.FromRgb(125, 218, 88)), 3);

        int intensityPercentageTo = (int)Math.Round((0.3 * (double)exercise.RPMMed) + (0.7 * (double)exercise.ResistancePercentage) + 20);

        InsertDataPoint(new DataPoint(_dataPoints[_currentSegmentIndex + 3].Time + ((double)exercise.DurationMin / 2), intensityPercentageTo, (double)exercise.DurationMin / 2, Microsoft.Maui.Graphics.Color.FromRgb(125, 218, 88)), 4);
    }

    private void InsertDataPoint(DataPoint newPoint, int indexToIncrement)
    {
        if ((_currentSegmentIndex + 1) % 2 == 0)
            indexToIncrement--;

        // Insertar el nuevo punto en la lista.
        _dataPoints.Insert(_currentSegmentIndex + indexToIncrement, newPoint);

        // Ajustar los tiempos de los puntos siguientes.
        for (int i = _currentSegmentIndex + indexToIncrement; i < _dataPoints.Count; i++)
        {
            _dataPoints[i].Time = _dataPoints[i - 1].Time + _dataPoints[i].Duration;
        }
    }

    public void UpdateTargetIntensity(int newIntensity)
    {
        _dataPoints[_currentSegmentIndex+1].Intensity = newIntensity;
    }
    
    private async void StartAnimation()
     {
        _stopwatch.Start();

        while (_currentSegmentIndex < _dataPoints.Count - 1)
        {
            double segmentDuration = _dataPoints[_currentSegmentIndex + 1].Duration * 60; // Convertir minutos a segundos
            double startIntensity = 0;
            double endIntensity = 0;

            if (!_reconnected)
            {
                _reconnectionActualExerciseTimeSpan = TimeSpan.Zero;
                _stopwatch.Restart();
            }
            else
                _reconnected = false;

            while (_stopwatch.Elapsed.TotalSeconds + _reconnectionActualExerciseTimeSpan.TotalSeconds < segmentDuration)
            {
                if (_drawable.CurrentSegmentIndex != _currentSegmentIndex)
                {
                    // Reconfigurar el segmento si cambia el índice actual
                    segmentDuration = _dataPoints[_currentSegmentIndex + 1].Duration * 60;
                    _reconnectionActualExerciseTimeSpan = TimeSpan.Zero;

                    if (!_stopwatch.IsRunning)
                    {
                        _stopwatch.Restart();
                        _stopwatch.Stop();
                    }
                    else
                        _stopwatch.Restart();
                }

                if (_stopwatch.IsRunning)
                {
                    // Calcula el progreso basado en el tiempo real transcurrido
                    double elapsedTime = _stopwatch.Elapsed.TotalSeconds + _reconnectionActualExerciseTimeSpan.TotalSeconds;
                    _drawable.Progress = elapsedTime / segmentDuration;

                    _drawable.CurrentSegmentIndex = _currentSegmentIndex;
                    graphicsView.Invalidate(); // Redibuja la vista gráfica

                    startIntensity = _dataPoints[_currentSegmentIndex].Intensity;
                    endIntensity = _dataPoints[_currentSegmentIndex + 1].Intensity;

                    SetEnergyZoneText((int)(startIntensity + (endIntensity - startIntensity) * _drawable.Progress));
                }

                await Task.Delay(16); // Espera pequeña para suavizar la animación (~60 FPS)
            }

            // Asegúrate de que el segmento finalice completamente
            _drawable.Progress = 1.0;
            graphicsView.Invalidate();

            if (segmentDuration > 0)
                _currentSegmentIndex++;
        }

        _stopwatch.Stop(); // Detén el medidor al final

        OnModalClosedTask.SetResult(true);

        await DisplayAlert("Sesión Finalizada", "La sesión ha finalizado. ¿Desea volver a la pantalla principal?", "Si");

        var messageReceived = await _remoteControlViewModel.SendFinishSessionMessage();

        if (messageReceived)
        {
            await _navigationServices.GoBackAsync();
        }
    }

    private void SetEnergyZoneText(int intensityValue)
    {
        if (intensityValue >= 50 && intensityValue <= 60)
        {
            lblCurrentEnergyZone.Text = $"{intensityValue}%-Recuperación";
            lblCurrentEnergyZone.TextColor = Color.FromRgb(27, 128, 170);
        }
        else if (intensityValue >= 60 && intensityValue <= 70)
        {
            lblCurrentEnergyZone.Text = $"{intensityValue}%-Fondo";
            lblCurrentEnergyZone.TextColor = Color.FromRgb(107, 170, 30);
        }
        else if (intensityValue >= 70 && intensityValue <= 80)
        {
            lblCurrentEnergyZone.Text = $"{intensityValue}%-Fuerza";
            lblCurrentEnergyZone.TextColor = Color.FromRgb(224, 193, 0);
        }
        else if (intensityValue >= 80 && intensityValue <= 90)
        {
            lblCurrentEnergyZone.Text = $"{intensityValue}%-Intervalos";
            lblCurrentEnergyZone.TextColor = Color.FromRgb(225, 118, 0);

        }
        else if (intensityValue >= 90 && intensityValue <= 110)
        {
            lblCurrentEnergyZone.Text = $"{intensityValue}%-Día de la Carrera";
            lblCurrentEnergyZone.TextColor = Color.FromRgb(219, 17, 28);
        }
    }

    private bool _navigationBack = false;

    private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
    {
        var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
        Shell.Current.Navigating -= OnNavigating;

        if (e.Source == ShellNavigationSource.Pop)
        {
            if (!OnModalClosedTask.Task.IsCompleted)
            {
                //// Cancelar la navegación                
                //e.Cancel();

                //if (!_navigationBack)
                //{
                //    _navigationBack = true;
                //    // Realizar tu validación aquí (por ejemplo, mostrar un diálogo al usuario)
                //    bool userWantsToGoBack = await DisplayAlert("Confirmación", "¿Estas seguro que deseas la reproducción de la sesión?", "Sí", "No");

                //    if (userWantsToGoBack)
                //    {
                //        _remoteControlViewModel.CancelSession();
                        
                //        await _navigationServices.GoBackAsync();
                //        OnModalClosedTask.SetResult(false);
                //    }

                //    _navigationBack = false;
                //}
            }
        }

        Shell.Current.Navigating += OnNavigating;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.Current.Navigating -= OnNavigating;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.Current.Navigating += OnNavigating;
        OnModalClosedTask = new TaskCompletionSource<bool>();
    }
}