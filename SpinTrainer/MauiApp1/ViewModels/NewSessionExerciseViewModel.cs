using System.Collections.ObjectModel;
using System.Windows.Input;
using SERVICES.ExerciseServices;
using SERVICES.SessionServices;
using ENTITYS;
using UTILITIES.ToastMessagesUtility;
using ENTITYS.DTOs;
using SERVICES.NavigationServices;

namespace SpinningTrainer.ViewModels
{
    public class NewSessionExerciseViewModel : ViewModelBase
    {
        private SessionEntity _session;
        private ExerciseEntity _selectedExercise;
        private SessionExercisesEntity _selectedSessionExercise;

        private ObservableCollection<SessionExercisesEntity> _selectedExercisesList = new ObservableCollection<SessionExercisesEntity>();
        private ObservableCollection<ExerciseEntity> _exercisesList = new ObservableCollection<ExerciseEntity>();
        private string _energyZoneFrom;
        private string _energyZoneTo;
        private ObservableCollection<HandsPositionsDto> _handsPositions = new ObservableCollection<HandsPositionsDto>();

        private int _idMovimiento;
        private int _posicionManos;        
        private int _rpmMed;
        private int _rpmFin;
        private int _resistancePercentage;
        private int _durationMin;
        private int _editIndex;
        private int _selectedHandsPositionIndex;
        private short _tipoEjercicio;
        private HandsPositionsDto _selectedHandsPosition;
        private bool _editionExerciseEnable;
        private bool _editingSession;

        public SessionEntity Session
        {
            get => _session;
            set
            {
                _session = value;
                OnPropertyChanged(nameof(Session));                
            }
        }
        public int IDMovimiento
        {
            get => _idMovimiento;
            set
            {
                _idMovimiento = value;
                OnPropertyChanged(nameof(IDMovimiento));
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
            }
        }
        public int PosicionManos
        {
            get => _posicionManos;
            set
            {
                _posicionManos = value;
                OnPropertyChanged(nameof(PosicionManos));
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
            }
        }
        public short TipoEjercicio
        {
            get => _tipoEjercicio;
            set
            {
                _tipoEjercicio = value;
                OnPropertyChanged(nameof(TipoEjercicio));
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
            }
        }        
        public int RPMMed
        {
            get => _rpmMed;
            set
            {
                _rpmMed = value;
                OnPropertyChanged(nameof(RPMMed));
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
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
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
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
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
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
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
            }
        }        
        public ExerciseEntity SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                if(_selectedExercise != value) 
                {
                    _selectedExercise = value;
                    OnPropertyChanged(nameof(SelectedExercise));
                    OnPropertyChanged(nameof(HasSelectedExercise));
                    SelectExercise();
                    ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                    ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public HandsPositionsDto SelectedHandsPosition 
        {
            get => _selectedHandsPosition; 
            set 
            {
                _selectedHandsPosition = value;
                OnPropertyChanged(nameof(SelectedHandsPosition));
                ((ViewModelCommand)AddSessionExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ModifySessionExerciseCommand).RaiseCanExecuteChanged();
            } 
        }
        public SessionExercisesEntity SelectedSessionExercise
        {
            get => _selectedSessionExercise;
            set
            {
                _selectedSessionExercise = value;
                OnPropertyChanged(nameof(SelectedSessionExercise));
            }
        }

        public ObservableCollection<SessionExercisesEntity> SelectedExercisesList
        {
            get => _selectedExercisesList;
            set
            {
                _selectedExercisesList = value;
                OnPropertyChanged(nameof(SelectedExercisesList));                
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
        public ObservableCollection<HandsPositionsDto> HandsPositions
        {
            get => _handsPositions;
            set
            {
                _handsPositions = value;
                OnPropertyChanged(nameof(HandsPositions));
            }
        }        
        public bool EditionExerciseEnable 
        {
            get => _editionExerciseEnable; 
            set 
            {
                _editionExerciseEnable = value; 
                OnPropertyChanged(nameof(EditionExerciseEnable));  
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
                if (SelectedHandsPosition != null)
                    return true;
                else
                    return false;
            }
        }
        public bool NotHandsPositionAreSelected
        {
            get => !HandsPositionAreSelected;
        }
        public int SelectedHandsPositionIndex 
        {
            get => _selectedHandsPositionIndex; 
            set 
            {
                _selectedHandsPositionIndex = value;
                OnPropertyChanged(nameof(SelectedHandsPositionIndex));
            }
        }

        public bool SaveSessionSuccesfully = false;

        private readonly IExerciseServices _exerciseServices;
        private readonly ISessionServices _sessionServices;
        private readonly INavigationServices _navigationServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;

        public ICommand AddSessionExerciseCommand { get; }
        public ICommand ModifySessionExerciseCommand { get; }
        public ICommand SaveSessionCommand { get; }
        public ICommand RemoveSessionExerciseCommand { get; }

        public NewSessionExerciseViewModel(ISessionServices sessionServices,
                                           IExerciseServices exerciseServices,
                                           INavigationServices navigationServices,
                                           IToastMessagesUtility toastMessagesUtility)
        {
            _sessionServices = sessionServices;
            _exerciseServices = exerciseServices;
            _navigationServices = navigationServices;
            _toastMessagesUtility = toastMessagesUtility;

            AddSessionExerciseCommand = new ViewModelCommand(ExecuteAddSessionExerciseCommand, CanExecuteAddSessionExerciseCommand);
            ModifySessionExerciseCommand = new ViewModelCommand(ExecuteModifySessionExerciseCommand, CanExecuteModifySessionExerciseCommand);
            SaveSessionCommand = new ViewModelCommand(ExecuteSaveSessionCommand);
            RemoveSessionExerciseCommand = new ViewModelCommand(ExecuteRemoveSessionExerciseCommand);

            LoadExercisesAsync();
        }

        public void SetSessionAndIsEditingValue(SessionEntity session, bool isEditing)
        {
            Session = session;
            _editingSession = isEditing;

            if (isEditing)
            {
                SelectedExercisesList = new ObservableCollection<SessionExercisesEntity>(Session.SessionExercises);
            }
        }

        private bool CanExecuteModifySessionExerciseCommand(object obj)
        {
            int intensityPercentageFrom = (int)Math.Round((0.3 * (double)this.RPMMed) + (0.7 * (double)this.ResistancePercentage) + 20);
            int intensityPercentageTo = (int)Math.Round((0.3 * (double)this.RPMFin) + (0.7 * (double)this.ResistancePercentage) + 20);

            if (SelectedExercise == null || SelectedHandsPosition == null || DurationMin == 0 ||
                RPMFin == 0 || RPMMed == 0 || ResistancePercentage < 20 || intensityPercentageFrom < 50 || intensityPercentageTo > 100)
                return false;
            else
                return true;
        }

        private bool CanExecuteAddSessionExerciseCommand(object obj)
        {
            if (SelectedExercise == null || SelectedHandsPosition == null || DurationMin == 0 ||
                RPMFin == 0 || RPMMed == 0 || ResistancePercentage < 20)
                return false;
            else
                return true;
        }

        private async void ExecuteAddSessionExerciseCommand(object obj)
        {
            int totalTimeExercises = DurationMin;

            foreach (var item in SelectedExercisesList)
            {
                totalTimeExercises += item.DurationMin;
            }            

            if (totalTimeExercises > this.Session.Duration)
            {
                await _toastMessagesUtility.ShowMessage("El tiempo total de los ejercicios excede al de la sesión.");
            }
            else 
            {
                SessionExercisesEntity newSessionExercise = new SessionExercisesEntity
                {
                    ExerciseID = this.SelectedExercise.ID,
                    DescripMov = this.SelectedExercise.Descrip,
                    HandsPosition = SelectedHandsPosition.Number,
                    ResistancePercentage = ResistancePercentage,
                    RPMMed = RPMMed,
                    RPMFin = RPMFin,
                    DurationMin = DurationMin
                };

                SelectedExercisesList.Add(newSessionExercise);
                ClearSelection();
                await _toastMessagesUtility.ShowMessage("Ejercicio agregado éxitosamente.");
                for (int i = 0; i < 5; i++)
                {
                    await _navigationServices.GoBackAsync();
                }
            }            
        }

        private async void ExecuteModifySessionExerciseCommand(object obj)
        {
            int totalTimeExercises = DurationMin;

            foreach (var item in SelectedExercisesList)
            {
                totalTimeExercises += item.DurationMin;
            }
            totalTimeExercises -= SelectedExercisesList[_editIndex].DurationMin;

            if (totalTimeExercises > this.Session.Duration)
            {
                await _toastMessagesUtility.ShowMessage("El tiempo total del ejercicio excede al de la sesión.");
            }
            else
            {
                SelectedExercisesList[_editIndex] = new SessionExercisesEntity
                {
                    ExerciseID = this.SelectedExercise.ID,
                    DescripMov = this.SelectedExercise.Descrip,
                    HandsPosition = this.SelectedHandsPosition.Number,
                    ResistancePercentage = this.ResistancePercentage,
                    RPMMed = this.RPMMed,
                    RPMFin = this.RPMFin,
                    DurationMin = this.DurationMin
                };

                await App.Current.MainPage.Navigation.PopAsync();
                ClearSelection();
            }            
        }

        private async void ExecuteSaveSessionCommand(object obj)
        {
            try
            {
                int totalExercisesTime = 0;
                
                foreach (var item in SelectedExercisesList)
                {
                    totalExercisesTime += item.DurationMin;
                }

                if(totalExercisesTime == Session.Duration)
                {
                    if(Session.SessionExercises == null)
                    {
                        Session.SessionExercises = new List<SessionExercisesEntity>(SelectedExercisesList);
                    }
                    else
                    {
                        Session.SessionExercises.Clear();

                        foreach (var exercise in SelectedExercisesList)
                        {
                            Session.SessionExercises.Add(exercise);
                        }
                    }

                    if (_editingSession)
                    {
                        var (sessionUpdated,errorMessage) = await _sessionServices.Update(this.Session);

                        if (sessionUpdated)
                        {
                            await _toastMessagesUtility.ShowMessage("Sesión actualizada exitosamente.");
                            
                            SaveSessionSuccesfully = true;
                            
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            await _toastMessagesUtility.ShowMessage(errorMessage);
                        }
                    }
                    else
                    {
                        var (sessionInserted, errorMessage) = await _sessionServices.Add(this.Session);

                        if(sessionInserted)
                        {
                            await _toastMessagesUtility.ShowMessage("Sesión creada exitosamente.");

                            SaveSessionSuccesfully = true;
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            await _toastMessagesUtility.ShowMessage(errorMessage);
                        }
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Tiempo Insuficiente", "La duración total de los ejercicios ingresados no es igual a la duración de la sesión", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                
                await _toastMessagesUtility.ShowMessage(ex.Message);
            }
        }

        private async void ExecuteRemoveSessionExerciseCommand(object obj)
        {
            SelectedExercisesList.RemoveAt(_editIndex);
            ClearSelection();
            await App.Current.MainPage.Navigation.PopAsync();
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
                    HandsPositions.Add(new HandsPositionsDto() { Number = item});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        public async void EnableEdit(SessionExercisesEntity selectedSessionExercise, int selectedSessionExerciseIndex)
        {
            var (exercise, operationComplete, errorMessage) = await _exerciseServices.GetById(selectedSessionExercise.ExerciseID);

            this.EditionExerciseEnable = true;            
            this.SelectedExercise = ExercisesList.FirstOrDefault(item => item.Descrip == exercise.Descrip);
            this._editIndex = selectedSessionExerciseIndex;
            this.SelectedHandsPositionIndex = HandsPositions.IndexOf(HandsPositions.FirstOrDefault(e => e.Number == selectedSessionExercise.HandsPosition.ToString()));
            this.SelectedHandsPosition = HandsPositions.ElementAt(SelectedHandsPositionIndex);
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
            this.SelectedHandsPositionIndex = 0;
            //this.SelectedHandsPosition = null;
            this.SelectedExercise = null;            
            this.EditionExerciseEnable = false;
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