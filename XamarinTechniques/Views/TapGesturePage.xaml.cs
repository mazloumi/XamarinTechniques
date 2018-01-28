using Xamarin.Forms;

namespace XamarinTechniques
{
    public partial class TapGesturePage : ContentPage
    {
        public TapGesturePage()
        {
            InitializeComponent();
        }

        void Entry_Focused(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (entry.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}