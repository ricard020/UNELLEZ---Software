using SpinningTrainerTV.ViewModelsTV;
using ENTITYS;

namespace SpinningTrainerTV.ViewTV
{
    public partial class RequestUserPINViewTV : ContentPage
    {
        public RequestUserPINViewTV()
        {
            InitializeComponent();
            SetupEntryEvents();
            pin1.Focus();
            // Código adicional
        }
        private void SetupEntryEvents()
        {
            pin1.TextChanged += (s, e) => FocusNextEntry(pin1, e.NewTextValue);
            pin2.TextChanged += (s, e) => FocusNextEntry(pin2, e.NewTextValue);
            pin3.TextChanged += (s, e) => FocusNextEntry(pin3, e.NewTextValue);
            pin4.TextChanged += (s, e) => FocusNextEntry(pin4, e.NewTextValue);
        }

        private async void FocusNextEntry(Entry currentEntry, string newTextValue)
        {
            if (!string.IsNullOrEmpty(newTextValue))
            {
                if (this.BindingContext is RequestUserPINViewModelTV viewModel)
                {
                    switch (currentEntry)
                    {
                        case Entry _ when currentEntry == pin1 && currentEntry.Text.Length == 2:
                            viewModel.CharPin1 = newTextValue;
                            viewModel.CharPin2 = viewModel.CharPin1.Substring(1, 1);
                            viewModel.CharPin1 = viewModel.CharPin1.Substring(0, 1);
                            pin1.Text = viewModel.CharPin1;
                            pin2.Focus();
                            break;
                        case Entry _ when currentEntry == pin2 && currentEntry.Text.Length == 2:
                            viewModel.CharPin2 = newTextValue;
                            viewModel.CharPin3 = viewModel.CharPin2.Substring(1, 1);
                            viewModel.CharPin2 = viewModel.CharPin2.Substring(0, 1);
                            pin2.Text = viewModel.CharPin2;
                            pin3.Focus();
                            break;
                        case Entry _ when currentEntry == pin3 && currentEntry.Text.Length == 2:
                            var finalChar = viewModel.CharPin3.Substring(1, 1);
                            viewModel.CharPin3 = newTextValue;
                            viewModel.CharPin3 = viewModel.CharPin3.Substring(0, 1);
                            pin3.Text = viewModel.CharPin3;
                            viewModel.CharPin4 = finalChar;
                            pin4.Text = viewModel.CharPin4;
                            pin4.Focus();
                            break;
                        case Entry _ when currentEntry == pin4:
                            await Task.Delay(100);
                            viewModel.ValidatePIN();
                            break;
                    }
                }
            }
        }

        private void OnPinEntryCompleted(object sender, EventArgs e)
        {
            Entry currentEntry = (Entry)sender;
            FocusNextEntry(currentEntry, currentEntry.Text);
        }

        private void OnPinEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry currentEntry = (Entry)sender;
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                switch (currentEntry)
                {
                    case Entry _ when currentEntry == pin2:
                        pin1.Focus();
                        break;
                    case Entry _ when currentEntry == pin3:
                        pin2.Focus();
                        break;
                    case Entry _ when currentEntry == pin4:
                        pin3.Focus();
                        break;
                }
            }
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            pin1.Focus();
        }
    }
}