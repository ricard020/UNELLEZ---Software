using System.Collections.ObjectModel;
using System.Windows.Input;
using ENTITYS;
using SpinningTrainer.ViewModels;
using SpinningTrainer.Views;
using SERVICES.SessionServices;
using SERVICES.UserServices;
using UTILITIES.ToastMessagesUtility;
using SERVICES.NavigationServices;


namespace SpinningTrainer.ViewModel
{
    public class SessionViewModel : ViewModelBase
    {
        //Esta propiedad es en caso de que la sesion se este editando, para almacenar aqui los ejercicios de la sesion editada.
        private ICollection<SessionExercisesEntity> _sessionEditedExercisesList;

        private string _descrip;
        private DateTime _selectedCreationDate;
        private DateTime _fechaI;
        private TimeSpan _timeI;
        private int _duracion;
        private bool _isEditing;
        private int _editingSessionID;

        public string Descrip
        {
            get => _descrip;
            set
            {
                _descrip = value;
                OnPropertyChanged(nameof(Descrip));
                ((Command)AddSessionCommand).ChangeCanExecute();
            }
        }

        public DateTime FechaI
        {
            get => _fechaI;
            set
            {
                _fechaI = value;
                OnPropertyChanged(nameof(FechaI));
                ((Command)AddSessionCommand).ChangeCanExecute();
            }
        }

        public TimeSpan TimeI
        {
            get => _timeI;
            set
            {
                _timeI = value;
                OnPropertyChanged(nameof(TimeI));
            }
        }

        public int Duracion
        {
            get => _duracion;
            set
            {
                _duracion = value;
                OnPropertyChanged(nameof(Duracion));
                ((Command)AddSessionCommand).ChangeCanExecute();
            }
        }

        public ObservableCollection<SessionEntity> Sessions { get; set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly ISessionServices _sessionServices;
        private readonly IUserServices _userServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;
        private readonly INavigationServices _navigationServices;

        // Commands
        public ICommand AddSessionCommand { get; }

        public SessionViewModel(ISessionServices sessionServices, IUserServices userServices, IToastMessagesUtility toastMessagesUtility, INavigationServices navigationServices, IServiceProvider serviceProvider)
        {
            _sessionServices = sessionServices;
            _userServices = userServices;
            _toastMessagesUtility = toastMessagesUtility;
            _navigationServices = navigationServices;
            _serviceProvider = serviceProvider;
            AddSessionCommand = new Command(ExecuteAddSessionCommand, CanExecuteAddSessionCommand);

            Sessions = new ObservableCollection<SessionEntity>();
            
            FechaI = DateTime.Now;
        }

        // CanExecute
        private bool CanExecuteAddSessionCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Descrip) && Duracion > 0;
        }

        private bool CanExecuteDeleteSessionCommand()
        {
            return _editingSessionID > 0;
        }

        // Execute Commands
        private async void ExecuteAddSessionCommand(object obj)
        {
            try
            {
                var currentUser = _userServices.GetCurrentUser();

                var session = new SessionEntity
                {
                    TrainerID = currentUser.Id,
                    Descrip = Descrip,
                    DateC = DateTime.Now,
                    DateI = new DateTime(
                                FechaI.Year,
                                FechaI.Month,
                                FechaI.Day,
                                TimeI.Hours,
                                TimeI.Minutes,
                                TimeI.Seconds
                                         ),
                    Duration = Duracion
                };

                if (_isEditing)
                {
                    session.ID = _editingSessionID;
                    session.SessionExercises = _sessionEditedExercisesList;
                }

                var newSessionExercisesListViewModel = _serviceProvider.GetService<NewSessionExerciseViewModel>();

                newSessionExercisesListViewModel.SetSessionAndIsEditingValue(session, _isEditing);

                await _navigationServices.NavigateToAsync<NewSessionExercisesListView>(newSessionExercisesListViewModel);
            }
            catch(Exception ex)
            {
                await _toastMessagesUtility.ShowMessage(ex.Message);
            }
        }

        public void SetIsEditSession(bool isEditing, int editingSessionID)
        {
            _isEditing = isEditing;
            _editingSessionID = editingSessionID;

            if (isEditing)
            {
                LoadEditingSession();
            }
        }

        private async void LoadEditingSession()
        {
            var (session, operationComplete, errorMessage) = await _sessionServices.GetByID(_editingSessionID);

            if (operationComplete)
            {
                this.Descrip = session.Descrip;
                this.FechaI = session.DateI;
                this.TimeI = new TimeSpan(session.DateI.Hour, session.DateI.Minute, session.DateI.Second);
                this.Duracion = session.Duration;
                this._sessionEditedExercisesList = session.SessionExercises;
            }
            else
            {
                _toastMessagesUtility.ShowMessage(errorMessage);
            }
        }
    }
}
