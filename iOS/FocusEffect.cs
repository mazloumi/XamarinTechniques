using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinTechniques.iOS;

[assembly: ResolutionGroupName("com.yourcompany")]
[assembly: ExportEffect(typeof(FocusEffect), "FocusEffect")]
namespace XamarinTechniques.iOS
{
    public class FocusEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var entry = Element as Entry;
            entry.Focused += Entry_Focused;
        }

        protected override void OnDetached()
        {
            var entry = Element as Entry;
            entry.Focused -= Entry_Focused;
        }

        void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}