#if ANDROID
using Android.Views;
#endif

namespace SpinningTrainerTV.DPadNavigation
{
    interface IPageKeyDown
    {
        #if ANDROID
        public bool OnPageKeyDown(Keycode keyCode, KeyEvent e);
        #endif
    }
}
