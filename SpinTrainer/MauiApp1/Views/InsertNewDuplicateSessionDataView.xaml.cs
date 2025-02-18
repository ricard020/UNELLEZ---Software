using SpinningTrainer.ViewModels;

namespace SpinningTrainer.Views;

public partial class InsertNewDuplicateSessionDataView : ContentPage
{
    public TaskCompletionSource<bool> OnModalClosedTask { get; private set; } = new TaskCompletionSource<bool>();

	public InsertNewDuplicateSessionDataView()
	{
		InitializeComponent();		
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        OnModalClosedTask.TrySetResult(true);
    }
}