using System.Collections.ObjectModel;
using System.Windows.Input;
using SERVICES.ExerciseServices;
using SERVICES.SessionServices;
using ENTITYS;
using UTILITIES.ToastMessagesUtility;


namespace SpinningTrainer.ViewModels;

public class CustomExerciseTemplateViewModel : ContentPage
{
	public CustomExerciseTemplateViewModel()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}