using SpinningTrainer.ViewModel;
using SpinningTrainer.ViewModels;
using System.Globalization;
namespace SpinningTrainer.Views;

public partial class CustomExerciseTemplateView : ContentPage
{
    private CustomExerciseTemplateViewModel _CustomExerciseTemplateViewModel;

    public CustomExerciseTemplateView(CustomExerciseTemplateViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
        _CustomExerciseTemplateViewModel = viewModel;
    }
}