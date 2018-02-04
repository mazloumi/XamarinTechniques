using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinTechniques.Droid;

[assembly: ResolutionGroupName("com.yourcompany")]
[assembly: ExportEffect(typeof(FocusEffect), "FocusEffect")]
namespace XamarinTechniques.Droid
{
    public class FocusEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var entry = Element as Entry;
            entry.Focused += HandleEvent;
            entry.Unfocused += HandleEvent;
        }

        protected override void OnDetached()
        {
            var entry = Element as Entry;
            entry.Focused -= HandleEvent;
            entry.Unfocused -= HandleEvent;
        }

        void HandleEvent(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}