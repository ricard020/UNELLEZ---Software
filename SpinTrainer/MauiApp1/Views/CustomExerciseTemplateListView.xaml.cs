using Microsoft.Extensions.DependencyInjection;
using SERVICES.NavigationServices;
using SpinningTrainer.ViewModel;
using SpinningTrainer.ViewModels;
using System.Globalization;

namespace SpinningTrainer.Views;

public partial class CustomExerciseTemplateListView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IServiceProvider _serviceProvider;

    private ExerciseConfiguratorListViewModel _ExerciseConfiguratorListViewModel;
    public CustomExerciseTemplateListView(INavigationServices navigationServices, IServiceProvider serviceProvider)
    {
		InitializeComponent();
        InitializeComponent();

        _navigationServices = navigationServices;
        _serviceProvider = serviceProvider;
    }
    private async void btnAddCustomExercise_Clicked(object sender, EventArgs e)
    {
        var viewModel = _serviceProvider.GetService<CustomExerciseTemplateViewModel>();
        await _navigationServices.NavigateToAsync<CustomExerciseTemplateView>(viewModel);
    }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Obtener el ViewModel del BindingContext
        if (this.BindingContext is CustomExerciseTemplateViewModel viewModel)
        {
            //viewModel.EnableEdit(viewModel.SelectedSessionExercise, viewModel.SelectedExercisesList.IndexOf(viewModel.SelectedSessionExercise));

            await _navigationServices.NavigateToAsync<CustomExerciseTemplateView>(viewModel);
        }
    }
}
