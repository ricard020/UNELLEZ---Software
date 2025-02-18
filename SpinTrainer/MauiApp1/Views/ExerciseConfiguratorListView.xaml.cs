using Microsoft.Extensions.DependencyInjection;
using SERVICES.NavigationServices;
using SpinningTrainer.ViewModel;
using SpinningTrainer.ViewModels;
using System.Globalization;

namespace SpinningTrainer.Views;

public partial class ExerciseConfiguratorListView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IServiceProvider _serviceProvider;


    private ExerciseConfiguratorListViewModel _ExerciseConfiguratorListViewModel;
    public ExerciseConfiguratorListView(INavigationServices navigationServices, IServiceProvider serviceProvider)
    {
         InitializeComponent();

        _navigationServices = navigationServices;
        _serviceProvider = serviceProvider;
    }
    private async void btnAddConfigExercise_Clicked(object sender, EventArgs e)
        {
        var viewModel = _serviceProvider.GetService<ExerciseConfiguratorViewModel>();
        await _navigationServices.NavigateToAsync<ExerciseConfiguratorView>(viewModel);
        }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Obtener el ViewModel del BindingContext
        if (this.BindingContext is ExerciseConfiguratorViewModel viewModel)
        {
            //viewModel.EnableEdit(viewModel.SelectedSessionExercise, viewModel.SelectedExercisesList.IndexOf(viewModel.SelectedSessionExercise));

            await _navigationServices.NavigateToAsync<ExerciseConfiguratorView>(viewModel);
        }
    }
}