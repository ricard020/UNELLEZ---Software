using System.Windows.Input;
using ENTITYS;
using SERVICES.CompanyDataService;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainer.ViewModels
{
    public class CompanyDataViewModel : ViewModelBase
    { 
        private string _rif;
        private string _descrip;
        private string _direc;
        private ImageSource _logo;
        private byte[] _logoBytes;
            
        private bool _editEnable;

        private readonly ICompanyDataService _companyDataService;
        private readonly IToastMessagesUtility _toastMessagesUtility;

        public CompanyDataViewModel(ICompanyDataService companyDataService, IToastMessagesUtility toastMessagesUtility)
        {
            _companyDataService = companyDataService;
            _toastMessagesUtility = toastMessagesUtility;
            SaveDataCommand = new Command(async () => await ExecuteSaveDataCommand(), CanExecuteSaveDataCommand);
            SearchImageCommand = new Command(async () => await ExecuteSearchImageCommand());
            EnableEditCommand = new Command(ExecuteEnableEditCommand);
            CancelEditCommand = new Command(ExecuteCancelEditCommand);
            LoadData();            
        }

        public string Rif
        {
            get => _rif;
            set
            {
                _rif = value;
                OnPropertyChanged(nameof(Rif));
                ((Command)SaveDataCommand).ChangeCanExecute();
            }
        }

        public string Descrip
        {
            get => _descrip;
            set
            {
                _descrip = value;
                OnPropertyChanged(nameof(Descrip));
                ((Command)SaveDataCommand).ChangeCanExecute();
            }
        }

        public string Direc
        {
            get => _direc;
            set
            {
                _direc = value;
                OnPropertyChanged(nameof(Direc));
                ((Command)SaveDataCommand).ChangeCanExecute();
            }
        }

        public ImageSource Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                OnPropertyChanged(nameof(Logo));
            }
        }

        public byte[] LogoBytes
        {
            get => _logoBytes;
            set
            {
                _logoBytes = value;
                OnPropertyChanged(nameof(LogoBytes));
            }
        }

        public bool EditEnable
        {
            get => _editEnable;
            set
            {
                _editEnable = value;
                OnPropertyChanged(nameof(EditEnable));
            }
        }

        public ICommand EnableEditCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand SearchImageCommand { get; }

        private bool CanExecuteSaveDataCommand()
        {
            return !string.IsNullOrWhiteSpace(Rif) && !string.IsNullOrWhiteSpace(Descrip) && !string.IsNullOrWhiteSpace(Direc);
        }

        private async Task ExecuteSaveDataCommand()
        {
            try
            {
                var companyData = new CompanyDataEntity
                {
                    RIF = Rif,
                    Descrip = Descrip,
                    Direc = Direc,
                    Logo  = LogoBytes
                };

                var(operationComplete, errorMessage) = await _companyDataService.SaveCompanyDataAsync(companyData);

                if (operationComplete)
                {
                    await _toastMessagesUtility.ShowMessage("Datos de la empresa guardados correctamente.");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await _toastMessagesUtility.ShowMessage("ERROR: "+errorMessage);
                }
            }
            catch (Exception ex)
            {
                await _toastMessagesUtility.ShowMessage("ERROR: " + ex.Message);
            }
        }

        private async Task ExecuteSearchImageCommand()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecciona una imagen"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        LogoBytes = memoryStream.ToArray();
                        if (LogoBytes.Length > 2048 )
                        {
                            throw new Exception("La imagen seleccionada es demasiado grande. Por favor, elige una imagen más pequeña.");
                        }
                    }
                    Logo = ImageSource.FromStream(() => new MemoryStream(LogoBytes));                     
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteEnableEditCommand()
        {
            EditEnable = true;
        }

        private void ExecuteCancelEditCommand()
        {
            EditEnable = false;
        }

        private async void LoadData()
        {
            try
            {
                var (companyData, operationComplete, errorMessage) = await _companyDataService.LoadCompanyData();

                if (operationComplete)
                {
                    if (companyData != null)
                    {
                        Rif = companyData.RIF;
                        Descrip = companyData.Descrip;
                        Direc = companyData.Direc;
                        LogoBytes = companyData.Logo;
                        Logo = ImageSource.FromStream(() => new MemoryStream(companyData.Logo));
                    }
                    else
                    {
                        await _toastMessagesUtility.ShowMessage("Datos no encontrados.");
                    }
                }
                else
                {
                    await _toastMessagesUtility.ShowMessage("ERROR: "+errorMessage);
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await _toastMessagesUtility.ShowMessage($"Error al cargar los datos de la empresa: {ex.Message}");
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
