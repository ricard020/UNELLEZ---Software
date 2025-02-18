using SERVICES.NavigationServices;
using SpinningTrainerTV.Resources.Charts;
using SpinningTrainerTV.ViewModelsTV;
using System.Diagnostics;
using System.Net;

namespace SpinningTrainerTV.ViewTV
{
    public partial class PlaySessionViewTV : ContentPage
    {
        private bool _finishedSession = false;

        private double _recoveryZoneDuration = 0;
        private double _enduranceZoneDuration = 0;
        private double _tempoZoneDuration = 0;
        private double _thresholdZoneDuration = 0;
        private double _maximalZoneDuration = 0;

        private GraphDrawable _drawable;
        private List<DataPoint> _dataPoints;

        private int _currentSegmentIndex = 0;

        private Stopwatch _stopwatch = new Stopwatch();

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationServices _navigationServices;

        private PlaySessionViewModelTV _viewModelTV;

        public PlaySessionViewTV(IServiceProvider serviceProvider, INavigationServices navigationServices)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _navigationServices = navigationServices;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            var viewModel = (PlaySessionViewModelTV)this.BindingContext;
            _viewModelTV = viewModel;
            viewModel.GetCurrentGraphData += ipToResponse => MainThread.BeginInvokeOnMainThread(() => GetCurrentGraphData(viewModel, ipToResponse));
            viewModel.StartGraphAnimation += () => MainThread.BeginInvokeOnMainThread(() => StartAnimation());
            viewModel.NextExercise += () => MainThread.BeginInvokeOnMainThread(() => NextExercise());
            viewModel.PreviousExercise += () => MainThread.BeginInvokeOnMainThread(() => PreviousExercise());
            viewModel.PauseOrPlayGraphAnimation += () => 
                MainThread.BeginInvokeOnMainThread(() => 
                {
                    if (_stopwatch.IsRunning)
                        _stopwatch.Stop();
                    else
                        _stopwatch.Start();
                });
            viewModel.InsertRestingExercise += () => MainThread.BeginInvokeOnMainThread(() => InsertRestingExercise());
            viewModel.ModifyExerciseIntensity += newIntensity => MainThread.BeginInvokeOnMainThread(() => UpdateTargetIntensity(newIntensity));

            _dataPoints = viewModel.LoadExercisesInChart();
            _drawable = new GraphDrawable(_dataPoints);
            
            graphicsView.Drawable = _drawable;
        }

        private void SetEnergyZoneText(int intensityValue, double time)
        {
            if (intensityValue >= 50 && intensityValue <= 60)
            {
                lblCurrentEnergyZone.Text = "Recuperación";
                _recoveryZoneDuration += time;
            }
            else if (intensityValue >= 60 && intensityValue <= 70)
            {
                lblCurrentEnergyZone.Text = "Fondo";
                _enduranceZoneDuration += time;
            }
            else if (intensityValue >= 70 && intensityValue <= 80)
            {
                lblCurrentEnergyZone.Text = "Fuerza";
                _tempoZoneDuration += time;
            }
            else if (intensityValue >= 80 && intensityValue <= 90)
            {
                lblCurrentEnergyZone.Text = "Intervalos";
                _thresholdZoneDuration += time;
            }
            else if (intensityValue >= 90 && intensityValue <= 110)
            {
                lblCurrentEnergyZone.Text = "Día de la Carrera";
                _maximalZoneDuration += time;
            }
        }

        private async void StartAnimation()
        {
            double actualDuration = 0;
            _stopwatch.Start();

            while (_currentSegmentIndex < _dataPoints.Count - 1)
            {
                double segmentDuration = _dataPoints[_currentSegmentIndex + 1].Duration * 60; // Convertir minutos a segundos
                double startIntensity = _dataPoints[_currentSegmentIndex].Intensity;
                double endIntensity = _dataPoints[_currentSegmentIndex + 1].Intensity;

                _stopwatch.Restart();
                actualDuration = 0;

                while (_stopwatch.Elapsed.TotalSeconds < segmentDuration)
                {
                    if (_finishedSession == true)
                    {
                        break;                       
                    }

                    if (_drawable.CurrentSegmentIndex != _currentSegmentIndex)
                    {
                        // Reconfigurar el segmento si cambia el índice actual
                        segmentDuration = _dataPoints[_currentSegmentIndex + 1].Duration * 60;
                        startIntensity = _dataPoints[_currentSegmentIndex].Intensity;
                        endIntensity = _dataPoints[_currentSegmentIndex + 1].Intensity;
                        actualDuration = 0;

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
                        double elapsedTime = _stopwatch.Elapsed.TotalSeconds;
                        _drawable.Progress = elapsedTime / segmentDuration;

                        _drawable.CurrentSegmentIndex = _currentSegmentIndex;
                        graphicsView.Invalidate(); // Redibuja la vista gráfica

                        SetEnergyZoneText((int)(startIntensity + (endIntensity - startIntensity) * _drawable.Progress), _stopwatch.Elapsed.TotalMinutes-actualDuration);
                        actualDuration = _stopwatch.Elapsed.TotalMinutes;
                    }
                    await Task.Delay(16); // Espera pequeña para suavizar la animación (~60 FPS)
                }

                if (_finishedSession == true)
                {
                    break;
                }

                // Asegúrate de que el segmento finalice completamente
                _drawable.Progress = 1.0;
                graphicsView.Invalidate();

                if (segmentDuration > 0)
                    _currentSegmentIndex++;
            }

            _stopwatch.Stop(); // Detén el medidor al final

            var viewmodel = _serviceProvider.GetService<SessionFinalResultsViewModelTV>();

            var (averageSpeed, totalDurationInMinutes, estimatedDistanceTraveledInKm, averageResistance) = _viewModelTV.GetSessionFinalResults();

            viewmodel.TotalSessionDuration = totalDurationInMinutes;
            viewmodel.AverageResistancePercentage = averageResistance;
            viewmodel.AverageSpeed = averageSpeed;
            viewmodel.EstimatedDistanceTraveled = estimatedDistanceTraveledInKm;

            viewmodel.SetSessionListViewModelTV(_viewModelTV.GetSessionListViewModelTV());

            var durations = new Dictionary<string, double>
            {
                { "Recuperación", Math.Round(_recoveryZoneDuration,2) },
                { "Fondo", Math.Round(_enduranceZoneDuration,2) },
                { "Fuerza", Math.Round(_tempoZoneDuration,2) },
                { "Intervalos", Math.Round(_thresholdZoneDuration,2) },
                { "Día de la Carrera", Math.Round(_maximalZoneDuration,2) }
            };

            var energyZoneTop5 = durations.OrderByDescending(x => x.Value).Take(5).ToList();

            for (int i = 0; i < energyZoneTop5.Count; i++)
            {
                if(i == 0)
                {
                    viewmodel.Top1EnergyZoneName = energyZoneTop5[i].Key;
                    viewmodel.Top1EnergyZoneDuration = energyZoneTop5[i].Value.ToString();
                }
                else if (i == 1)
                {
                    viewmodel.Top2EnergyZoneName = energyZoneTop5[i].Key;
                    viewmodel.Top2EnergyZoneDuration = energyZoneTop5[i].Value.ToString();
                }
                else if (i == 2)
                {
                    viewmodel.Top3EnergyZoneName = energyZoneTop5[i].Key;
                    viewmodel.Top3EnergyZoneDuration = energyZoneTop5[i].Value.ToString();
                }
                else if (i == 3)
                {
                    viewmodel.Top4EnergyZoneName = energyZoneTop5[i].Key;
                    viewmodel.Top4EnergyZoneDuration = energyZoneTop5[i].Value.ToString();
                }
                else if (i == 4)
                {
                    viewmodel.Top5EnergyZoneName = energyZoneTop5[i].Key;
                    viewmodel.Top5EnergyZoneDuration = energyZoneTop5[i].Value.ToString();
                }
            }

            await _navigationServices.NavigateToAsync<SessionFinalResultsViewTV>(viewmodel);
        }

        public void UpdateTargetIntensity(int newIntensity)
        {
            _dataPoints[_currentSegmentIndex + 1].Intensity = newIntensity;
        }

        private void InsertRestingExercise()
        {
            var viewmodel = (PlaySessionViewModelTV)this.BindingContext;

            var exercise = viewmodel.GetRestingExercise();

            int intensityPercentageFrom = (int)Math.Round((0.3 * (double)exercise.RPMFin) + (0.7 * (double)exercise.ResistancePercentage) + 20);

            InsertDataPoint(new DataPoint(_dataPoints[_currentSegmentIndex + 2].Time + ((double)exercise.DurationMin/2), intensityPercentageFrom, (double)exercise.DurationMin/2, Microsoft.Maui.Graphics.Color.FromRgb(125, 218, 88)),3);

            int intensityPercentageTo = (int)Math.Round((0.3 * (double)exercise.RPMMed) + (0.7 * (double)exercise.ResistancePercentage) + 20);

            InsertDataPoint(new DataPoint(_dataPoints[_currentSegmentIndex + 3].Time + ((double)exercise.DurationMin/2), intensityPercentageTo, (double)exercise.DurationMin/2, Microsoft.Maui.Graphics.Color.FromRgb(125, 218, 88)),4);
        }

        private void InsertDataPoint(DataPoint newPoint, int indexToIncrement)
        {
            if ((_currentSegmentIndex+1) % 2 == 0)
                indexToIncrement--;

            // Insertar el nuevo punto en la lista.
            _dataPoints.Insert(_currentSegmentIndex + indexToIncrement, newPoint);

            // Ajustar los tiempos de los puntos siguientes.
            for (int i = _currentSegmentIndex + indexToIncrement; i < _dataPoints.Count; i++)
            {
                _dataPoints[i].Time = _dataPoints[i - 1].Time + _dataPoints[i].Duration;
            }
        }

        private void PreviousExercise()
        {
            if (_drawable.CurrentSegmentIndex > 0)
            {
                if ((_currentSegmentIndex+1) % 2 != 0)
                    _currentSegmentIndex -= 2;
                else 
                    _currentSegmentIndex -= 3;
                graphicsView.Invalidate(); // Refresca la vista gráfica.
            }
        }

        private void NextExercise()
        {
            if (_drawable.CurrentSegmentIndex < _dataPoints.Count - 1)
            {
                if ((_currentSegmentIndex+1) % 2 != 0)
                    _currentSegmentIndex += 2;
                else
                    _currentSegmentIndex ++;

                graphicsView.Invalidate(); // Refresca la vista gráfica.
            }
        }

        private void GetCurrentGraphData(PlaySessionViewModelTV viewModel, IPEndPoint ipToResponse)
        {
            var startTime = DateTime.Now - _stopwatch.Elapsed;
            viewModel.SendCurrentSessioninPlayData(_dataPoints, _drawable, ipToResponse, startTime);
        }

        private void ContentPage_Unloaded(object sender, EventArgs e)
        {
            var viewModel = (PlaySessionViewModelTV)this.BindingContext;

            viewModel.StopTimer();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            _finishedSession = true;

            await Task.Delay(300);

            // Asegúrate de liberar el GraphDrawable
            if (_drawable is IDisposable disposableDrawable)
            {
                disposableDrawable.Dispose();
            }
            
            graphicsView.Drawable = null; // Eliminar referencia
        }

    }
}
