using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace SpinningTrainer.Views;

public partial class WebView : ContentPage
{
	public WebView()
    {
		InitializeComponent();
        wvPaginaWeb.Source = "https://spintrainer.vercel.app";
    }
}