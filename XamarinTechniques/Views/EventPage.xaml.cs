using Xamarin.Forms;

namespace XamarinTechniques
{
    public partial class EventPage : ContentPage
    {
        public EventPage()
        {
            InitializeComponent();
        }

        void HandleEvent(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}