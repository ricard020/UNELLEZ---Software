using ENTITYS;
using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class NewSessionExercisesListView : ContentPage
    {
        public TaskCompletionSource<bool> OnModalClosedTask;
        
        private readonly INavigationServices _navigationServices;

        public NewSessionExercisesListView(INavigationServices navigationServices)
        {
            InitializeComponent();
            Shell.Current.Navigating += OnNavigating;
            _navigationServices = navigationServices;
        }

        private async void btnAddSessionExercise_Clicked(object sender, EventArgs e)
        {            
            // Obtener el ViewModel del BindingContext
            if (this.BindingContext is NewSessionExerciseViewModel viewModel)
            {
                await _navigationServices.NavigateToAsync<SessionExerciseSelectionView>(viewModel);
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
                    if (!(currentPage is SessionExerciseFormView))
                    {
                        // Cancelar la navegación                
                        e.Cancel();

                        var viewModel = (NewSessionExerciseViewModel)this.BindingContext;

                        if (!viewModel.SaveSessionSuccesfully)
                        {
                            if (!_navigationBack)
                            {
                                _navigationBack = true;
                                // Realizar tu validación aquí (por ejemplo, mostrar un diálogo al usuario)
                                //bool userWantsToGoBack = await DisplayAlert("Confirmación", "¿Estas seguro que deseas cancelar los cambios?", "Sí", "No");

                                if (true)
                                {
                                    OnModalClosedTask.SetResult(false);
                                    await _navigationServices.GoBackAsync();
                                }

                                _navigationBack = false;
                            }
                        }
                        else
                        {
                            OnModalClosedTask.SetResult(true);
                            await _navigationServices.GoBackAsync();
                            await _navigationServices.GoBackAsync();
                        }
                    }
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

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el ViewModel del BindingContext
            if (this.BindingContext is NewSessionExerciseViewModel viewModel)
            {
                viewModel.EnableEdit(viewModel.SelectedSessionExercise, viewModel.SelectedExercisesList.IndexOf(viewModel.SelectedSessionExercise));

                await _navigationServices.NavigateToAsync<SessionExerciseFormView>(viewModel);
            }

            cvSelectedExercisesView.SelectionChanged -= CollectionView_SelectionChanged;
            cvSelectedExercisesView.SelectedItem = null;
            cvSelectedExercisesView.SelectionChanged += CollectionView_SelectionChanged;
        }
    }
}


