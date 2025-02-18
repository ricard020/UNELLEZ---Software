using SERVICES.DatabaseServices;
using SpinningTrainer.ViewModels;
using SERVICES.NavigationServices;
using SpinningTrainer.Views;
using SERVICES.SynchronizerServices;

namespace SpinTrainer.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private string[,] _tipsArray;
        private string _tipTitle;
        private string _tipText;
        private string _progressMessage;
        private double _progressValue = 0;

        public string ProgressMessage
        {
            get => _progressMessage;
            set
            {
                _progressMessage = value;
                OnPropertyChanged(nameof(ProgressMessage));
            }
        }
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }
        public string TipTitle
        {
            get => _tipTitle;
            set
            {
                _tipTitle = value;
                OnPropertyChanged(nameof(TipTitle));
            }
        }
        public string TipText
        {
            get => _tipText;
            set
            {
                _tipText = value;
                OnPropertyChanged(nameof(TipText));
            }
        }

        private readonly ISynchronizerServices _synchronizerServices;
        private readonly IDatabaseServices _databaseServices;
        private readonly INavigationServices _navigationServices;

        public SplashScreenViewModel(IDatabaseServices databaseServices, INavigationServices navigationServices, ISynchronizerServices synchronizerServices)
        {
            _databaseServices = databaseServices;
            _navigationServices = navigationServices;
            _synchronizerServices = synchronizerServices;
            InitializeArray();
        }

        public async Task LoadData()
        {
            UpdatesScreenTips();

            ProgressMessage = "Cargando... Comprobando archivos.";
            ProgressValue = 0.2;

            ProgressMessage = "Cargando... Comprobando conexión a la base de datos.";

            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "Application.cfg";
            string filePath = Path.Combine(executablePath, fileName);

            if (File.Exists(filePath))
            {
                UpdatesScreenTips();
                ProgressValue = 0.3;

                if(await _databaseServices.ApplyLocalBDMigrationAsync())
                {
                    ProgressValue = 0.5;
                    UpdatesScreenTips();

                    if (await _databaseServices.TestDataBaseConnectionAsync())
                    {
                        ProgressMessage = "Cargando... Conexión establecida, iniciando modo ONLINE.";
                        _databaseServices.SetIsOnlineValue(true);

                        await Task.Delay(1000);

                        ProgressValue = 0.8;
                        UpdatesScreenTips();

                        ProgressMessage = "Cargando... Migrando sesiones.";

                        var (operationComplete, errorMessage) = await _synchronizerServices.SynchronizeAsync();

                        if (operationComplete)
                        {
                            ProgressMessage = "Cargando... Migración completada. Iniciando.";
                        }
                        else
                        {
                            if(await Application.Current.MainPage.DisplayAlert("ERROR", "Ocurrió un error al migrar algunas sesiones del dispositivo móvil al servidor. ¿Desea continuar de todas formas?", "Si", "No"))
                            {
                                ProgressMessage = "Cargando... Migración fallida. Iniciando.";
                            }
                            else
                            {
                                Application.Current.Quit();
                            }
                        }
                    }
                    else
                    {
                        ProgressMessage = "Cargando... No hubo conexión, iniciando modo OFFLINE.";
                        _databaseServices.SetIsOnlineValue(false);
                    }

                    ProgressValue = 1;

                    await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("ERROR", "Error creando la base de datos local.", "OK");

                    Application.Current.Quit();
                }
            }
            else
            {
                UpdatesScreenTips();
                ProgressValue = 1;
                await Shell.Current.GoToAsync($"//{nameof(ConnectionView)}");
            }
        }

        private void UpdatesScreenTips()
        {
            var (tituloConsejo, consejo) = SearchTip();

            TipTitle = tituloConsejo;
            TipText = consejo;
        }

        private (string, string) SearchTip()
        {
            Random random = new Random(); // Crea una instancia de la clase Random
            int numeroAleatorio = random.Next(0, _tipsArray.GetLength(0)); // Genera un número aleatorio entre 0 y 14 (inclusive)

            return (_tipsArray[numeroAleatorio, 0], _tipsArray[numeroAleatorio, 1]);
        }

        private void InitializeArray()
        {
            _tipsArray = new string[,]
            {
            {"Individuos Inactivos", "Vigílalos de cerca para asegurar que entrenen a un nivel adecuado (puedes guiarlos desde fuera de la bicicleta)." },
            {"Individuos Inactivos", "Corrige su postura para que desarrollen una técnica de pedaleo adecuada." },
            {"Individuos Inactivos", "Mantén las clases sencillas, enfocándote en lo básico (posición, movimientos, manos, intensidad) para que no se sobrecarguen." },
            {"Individuos Que Buscan Recreación", "Ofrece clases variadas con desafíos de destreza para mantenerlos motivados." },
            {"Individuos Que Buscan Recreación", "Utiliza música y formatos de clase divertidos para que disfruten del ejercicio." },
            {"Individuos Que Buscan Rendimiento", "Implementa sesiones periodizadas y entrenamientos basados en la potencia o la frecuencia cardíaca." },
            {"Individuos Que Buscan Rendimiento", "Anímalos a compartir sus conocimientos y experiencias con el resto de la clase." },
            {"Individuos Que Buscan Rendimiento", "No te desanimes si los ciclistas experimentados entrenan a su propio ritmo." },
            {"Individuos Que Buscan Rendimiento", "Ofrece consejos sobre la forma y la técnica para mejorar la eficiencia y el rendimiento." },
            {"Poblaciones Especiales", "Ellos necesitan una atención especial y deben ser dirigidos para ir a su propio ritmo." },
            {"Poblaciones Especiales", "Préstales atención periódicamente para asegurarse si están realizando los ejercicios a una intensidad adecuada." },
            {"Preparación", "No esperes al último minuto. Selecciona o crea tu música y perfiles de Spinning con una semana de anticipación." },
            {"Preparación", "No des por sentado que todo está en orden. Comprueba la configuración adecuada de las bicicletas para alumnos nuevos y experimentados." },
            {"Preparación", "Refresca la memoria de los alumnos. Diles que no olviden traer una toalla y una botella de agua llena para mantenerse hidratados." },
            {"Preparación", "Pide a los alumnos que comiencen a pedalear tan pronto como sea posible para calentar sus piernas y prepararse para la sesión." },
            {"Preparación", "Prepara tu música y revisa tus perfiles de spinning con antelación." },
            {"Preparación", "Es posible que necesites modificar un plan, llega 15 minutos antes de la sesión." },
            {"Preparación", "Presenta una rutina divertida y desafiante." },
            {"Preparación", "Verifica el sonido y tú lista de músicas." },
            {"Preparación", "Chequea los ajustes de la bici que estén noveladas en el piso y estables." },
            {"Preparación", "Da la bienvenida a los nuevos alumnos, enséñales acerca del piñón fijo y la función del freno de emergencia." },
            {"Preparación", "Recuerda a los alumnos disponer de una toalla y botella de agua." },
            {"Preparación", "El programa spinning recomienda beber un total de aproximadamente un litro de líquido, antes, durante y después de una rutina." },
            {"Preparación", "Haz hincapié en que deben sentirse libres para ir a su propio ritmo." },
            {"Preparación", "Repasa brevemente los objetivos y el plan de la clase." },
            {"Preparación", "Pídele a los alumnos, que comiencen a pedalear para calentar sus piernas tan pronto como sea posible." },
            {"Preparación", "El monitoreo de la frecuencia cardíaca es importante para la seguridad y el rendimiento." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Utiliza música instrumental para iniciar y finalizar la clase, para establecer información de los estudiantes." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Música polirritmica como Electrónica o New Age: De esta manera tus estudiantes encuentran su propio ritmo." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Variedad de canciones y estados de ánimo: Atraerás diversos de alumnos." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Cambia periódicamente tu lista de reproducción, evitarás aburrimiento y proporcionaras frescura." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Algunas canciones emiten mensajes negativos y ofensivos, que pueden ser no motivadores para tus clases." },
            {"¿Cómo seleccionar la música para un perfil de spinning?", "Puedes solicitar sugerencias musicales a tus alumnos." },
            };
        }
    }
}
