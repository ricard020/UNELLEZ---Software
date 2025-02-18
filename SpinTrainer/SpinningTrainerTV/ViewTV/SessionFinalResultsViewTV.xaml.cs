using Microcharts;
using SkiaSharp;
using SpinningTrainerTV.ViewModelsTV;
namespace SpinningTrainerTV.ViewTV;

public partial class SessionFinalResultsViewTV : ContentPage
{
    private SessionFinalResultsViewModelTV _viewModelTV;

    public SessionFinalResultsViewTV()
	{
		InitializeComponent();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        var viewModel = (SessionFinalResultsViewModelTV)this.BindingContext;
        
        _viewModelTV = viewModel;

        _viewModelTV.WaitingForTheFinishedSessionMessage();
    }
}