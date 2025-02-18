using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using SpinningTrainerTV.DPadNavigation;

namespace SpinningTrainerTV
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            Page p = Shell.Current.CurrentPage;

            if (p is IPageKeyDown)
            {
                bool handled = (p as IPageKeyDown).OnPageKeyDown(keyCode, e);
                if (handled) return true;
            }

            return base.OnKeyDown(keyCode, e);
        }
    }
}
