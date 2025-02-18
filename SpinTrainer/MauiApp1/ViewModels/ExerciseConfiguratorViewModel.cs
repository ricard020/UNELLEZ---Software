using System.Collections.ObjectModel;
using System.Windows.Input;
using SERVICES.ExerciseServices;
using SERVICES.SessionServices;
using ENTITYS;
using UTILITIES.ToastMessagesUtility;
using SERVICES.ExerciseTemplateServices;
using SERVICES.UserServices;
using SERVICES.NavigationServices;

namespace SpinningTrainer.ViewModels
{
    public class ExerciseConfiguratorViewModel : ViewModelBase
    {
        private ExerciseEntity _selectedExercise;
        private ImageSource _selectedHandsPositionImage;
       
        private ObservableCollection<ExerciseEntity> _exercisesList = new ObservableCollection<ExerciseEntity>();
        private string _energyZoneFrom;
        private string _energyZoneTo;
        private ObservableCollection<string> _handsPositions = new ObservableCollection<string>();

        private int _idMovimiento;
        private int _posicionManos;
        private int _rpmMed;
        private int _rpmFin;
        private int _resistancePercentage;
        private int _durationMin;
        private int _editIndex;
        private short _tipoEjercicio;
        private string _selectedHandsPosition;

        public int IDMovimiento
        {
            get => _idMovimiento;
            set
            {
                _idMovimiento = value;
                OnPropertyChanged(nameof(IDMovimiento));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
            }
        }
        public int PosicionManos
        {
            get => _posicionManos;
            set
            {
                _posicionManos = value;
                OnPropertyChanged(nameof(PosicionManos));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
            }
        }
        public short TipoEjercicio
        {
            get => _tipoEjercicio;
            set
            {
                _tipoEjercicio = value;
                OnPropertyChanged(nameof(TipoEjercicio));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
            }
        }
        public int RPMMed
        {
            get => _rpmMed;
            set
            {
                _rpmMed = value;
                OnPropertyChanged(nameof(RPMMed));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
                CalculateIntensityPercentages();
            }
        }
        public int RPMFin
        {
            get => _rpmFin;
            set
            {
                _rpmFin = value;
                OnPropertyChanged(nameof(RPMFin));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
                CalculateIntensityPercentages();
            }
        }
        public int ResistancePercentage
        {
            get => _resistancePercentage;
            set
            {
                _resistancePercentage = value;
                OnPropertyChanged(nameof(ResistancePercentage));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
                CalculateIntensityPercentages();
            }
        }
        public int DurationMin
        {
            get => _durationMin;
            set
            {
                _durationMin = value;
                OnPropertyChanged(nameof(DurationMin));
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
            }
        }
        public ExerciseEntity SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                if (_selectedExercise != value)
                {
                    _selectedExercise = value;
                    OnPropertyChanged(nameof(SelectedExercise));
                    OnPropertyChanged(nameof(HasSelectedExercise));
                    SelectExercise();
                    ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string SelectedHandsPosition
        {
            get => _selectedHandsPosition;
            set
            {
                _selectedHandsPosition = value;
                OnPropertyChanged(nameof(SelectedHandsPosition));
                UpdateSelectedHandsPositionImage();
                ((ViewModelCommand)SaveRestingCommand).RaiseCanExecuteChanged();
            }
        }
        public ImageSource SelectedHandsPositionImage
        {
            get => _selectedHandsPositionImage;
            set
            {
                _selectedHandsPositionImage = value;
                OnPropertyChanged(nameof(SelectedHandsPositionImage));
                OnPropertyChanged(nameof(HandsPositionAreSelected));
                OnPropertyChanged(nameof(NotHandsPositionAreSelected));
            }
        }
        public ObservableCollection<ExerciseEntity> ExercisesList
        {
            get => _exercisesList;
            set
            {
                _exercisesList = value;
                OnPropertyChanged(nameof(ExercisesList));
            }
        }
        public string EnergyZoneFrom
        {
            get => _energyZoneFrom;
            set
            {
                _energyZoneFrom = value;
                OnPropertyChanged(nameof(EnergyZoneFrom));
            }
        }
        public string EnergyZoneTo
        {
            get => _energyZoneTo;
            set
            {
                _energyZoneTo = value;
                OnPropertyChanged(nameof(EnergyZoneTo));
            }
        }
        public ObservableCollection<string> HandsPositions
        {
            get => _handsPositions;
            set
            {
                _handsPositions = value;
                OnPropertyChanged(nameof(HandsPositions));
            }
        }
        public bool HasSelectedExercise
        {
            get => SelectedExercise != null ? true : false;
        }
        public bool HandsPositionAreSelected
        {
            get
            {
                if (SelectedHandsPositionImage != null)
                    return true;
                else
                    return false;
            }
        }
        public bool NotHandsPositionAreSelected
        {
            get => !HandsPositionAreSelected;
        }

        public bool SaveSessionSuccesfully = false;

        private readonly IExerciseServices _exerciseServices;
        private readonly IExerciseTemplateServices _exerciseTemplateServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;
        private readonly IUserServices _userServices;
        private readonly INavigationServices _navigationServices;

        public ICommand SaveRestingCommand { get; }

        public ExerciseConfiguratorViewModel(INavigationServices navigationServices, IExerciseTemplateServices exerciseTemplateServices, IToastMessagesUtility toastMessagesUtility, IUserServices userServices, IExerciseServices exerciseServices)
        {
            _exerciseServices = exerciseServices;
            _navigationServices = navigationServices;
            _exerciseTemplateServices = exerciseTemplateServices;
            _toastMessagesUtility = toastMessagesUtility;
            _userServices = userServices;
            SaveRestingCommand = new ViewModelCommand(ExecuteSaveRestingCommand, CanExecuteSaveRestingCommand);
   
            LoadExercisesAsync();
            LoadPreviousExercise();
        }

        private async void LoadPreviousExercise()
        {
            var (exercise, operationComplete, errorMessage) = await _exerciseTemplateServices.GetRestingExercise();

            if (operationComplete)
            {
                if (exercise != null)
                {
                    SelectedExercise = ExercisesList.FirstOrDefault(e => e.ID == exercise.ExerciseID);
                    SelectedHandsPosition = exercise.HandsPosition;
                    RPMFin = exercise.RPMFin;
                    RPMMed = exercise.RPMMed;
                    DurationMin = exercise.DurationMin;
                    ResistancePercentage = exercise.ResistancePercentage;
                }
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
                await _navigationServices.GoBackAsync();
            }
        }

        private bool CanExecuteSaveRestingCommand(object obj)
        {
            if (SelectedExercise == null || SelectedHandsPosition == null || DurationMin == 0 ||
                RPMFin == 0 || RPMMed == 0 || ResistancePercentage < 20)
                return false;
            else
                return true;
        }

        private async void ExecuteSaveRestingCommand(object obj)
        {
            var restingExercise = new ExerciseTemplateEntity()
            {
                DescripMov = SelectedExercise.Descrip,
                DurationMin = DurationMin,
                ExerciseID = SelectedExercise.ID,
                HandsPosition = SelectedHandsPosition,
                IsRestingExercise = (short)1,
                ResistancePercentage = ResistancePercentage,
                RPMFin = RPMFin,
                RPMMed = RPMMed,
                TemplateName = "Ejercicio de descanso",
                UserID = _userServices.GetCurrentUser().Id
            };

            var (operationComplete, errorMessage) = await _exerciseTemplateServices.SaveRestingExercise(restingExercise);

            if (operationComplete)
            {
                _toastMessagesUtility.ShowMessage("Operacion completada!");
                _navigationServices.GoBackAsync();
            }
            else
            {
                _toastMessagesUtility.ShowMessage(errorMessage);
            }
        }

        private async Task LoadExercisesAsync()
        {
            var (exercisesEnumerable, operationComplete, errorMessage) = await _exerciseServices.GetAll();

            if (operationComplete)
            {
                foreach (var item in exercisesEnumerable)
                {
                    ExercisesList.Add(item);
                }
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }
        }

        private void SelectExercise()
        {
            try
            {
                RPMMed = this.SelectedExercise.RPMMin;
                RPMFin = this.SelectedExercise.RPMMax;

                var arrayHandsPositions = this.SelectedExercise.HandsPositions.Split(",");

                HandsPositions.Clear();
                foreach (var item in arrayHandsPositions)
                {
                    HandsPositions.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateSelectedHandsPositionImage()
        {
            if (!string.IsNullOrEmpty(SelectedHandsPosition))
            {
                SelectedHandsPositionImage = ImageSource.FromFile($"Resources/Images/HandsPositions/hp{SelectedHandsPosition}.jpeg");
            }
        }

        public async void EnableEdit(SessionExercisesEntity selectedSessionExercise, int selectedSessionExerciseIndex)
        {
            var (exercise, operationComplete, errorMessage) = await _exerciseServices.GetById(selectedSessionExercise.ExerciseID);

            this.SelectedExercise = ExercisesList.FirstOrDefault(item => item.Descrip == exercise.Descrip);
            this._editIndex = selectedSessionExerciseIndex;
            this.SelectedHandsPosition = selectedSessionExercise.HandsPosition.ToString();
            this.ResistancePercentage = selectedSessionExercise.ResistancePercentage;
            this.RPMMed = selectedSessionExercise.RPMMed;
            this.RPMFin = selectedSessionExercise.RPMFin;
            this.DurationMin = selectedSessionExercise.DurationMin;
        }

        public void ClearSelection()
        {
            this.RPMFin = 0;
            this.RPMMed = 0;
            this.DurationMin = 0;
            this.ResistancePercentage = 0;
            this.SelectedHandsPosition = null;
            this.SelectedHandsPositionImage = null;
            this.SelectedExercise = null;
        }

        private void CalculateIntensityPercentages()
        {
            int intensityPercentageFrom = (int)Math.Round((0.3 * (double)RPMMed) + (0.7 * (double)ResistancePercentage) + 20);
            int intensityPercentageTo = (int)Math.Round((0.3 * (double)RPMFin) + (0.7 * (double)ResistancePercentage) + 20);

            string energyZoneFrom = "";
            string energyZoneTo = "";

            if (intensityPercentageFrom >= 50 && intensityPercentageFrom <= 60)
                energyZoneFrom = "Recuperación";
            else if (intensityPercentageFrom >= 60 && intensityPercentageFrom <= 70)
                energyZoneFrom = "Fondo";
            else if (intensityPercentageFrom >= 70 && intensityPercentageFrom <= 80)
                energyZoneFrom = "Fuerza";
            else if (intensityPercentageFrom >= 80 && intensityPercentageFrom <= 90)
                energyZoneFrom = "Intervalos";
            else if (intensityPercentageFrom >= 90 && intensityPercentageFrom <= 110)
                energyZoneFrom = "Día de la Carrera";

            if (intensityPercentageTo >= 50 && intensityPercentageTo <= 60)
                energyZoneTo = "Recuperación";
            else if (intensityPercentageTo >= 60 && intensityPercentageTo <= 70)
                energyZoneTo = "Fondo";
            else if (intensityPercentageTo >= 70 && intensityPercentageTo <= 80)
                energyZoneTo = "Fuerza";
            else if (intensityPercentageTo >= 80 && intensityPercentageTo <= 90)
                energyZoneTo = "Intervalos";
            else if (intensityPercentageTo >= 90 && intensityPercentageTo <= 110)
                energyZoneTo = "Día de la Carrera";

            EnergyZoneFrom = $"{intensityPercentageFrom}% - {energyZoneFrom}";
            EnergyZoneTo = $"{intensityPercentageTo}% - {energyZoneTo}";
        }
    }
}