using Microsoft.Extensions.DependencyInjection;
using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using SpinTrainer.ViewModels;
using System;
namespace SpinningTrainer.Views;

public partial class ConfiguratorMenuView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IServiceProvider _serviceProvider;


    public ConfiguratorMenuView(INavigationServices navigationServices, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _navigationServices = navigationServices;
        _serviceProvider = serviceProvider;
    }
    private async void btnCustomizeExercise_Clicked(object sender, EventArgs e)
    {
        var viewModel = _serviceProvider.GetService<ExerciseConfiguratorViewModel>();

        await _navigationServices.NavigateToAsync<ExerciseConfiguratorView>(viewModel);
    }
    private async void btnCustomizeTemplate_Clicked(object sender, EventArgs e)
    {
        var viewModel = _serviceProvider.GetService<CustomExerciseTemplateListViewModel>();

        await _navigationServices.NavigateToAsync<CustomExerciseTemplateListView>(viewModel);
    }
}
