using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinTechniques;
using XamarinTechniques.iOS;

[assembly: ExportRenderer(typeof(FocusEntry), typeof(FocusEntryRenderer))]

namespace XamarinTechniques.iOS
{
    public class FocusEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.Focused -= HandleEvent;
                e.OldElement.Unfocused -= HandleEvent;
            }

            if (e.NewElement != null)
            {
                e.NewElement.Focused += HandleEvent;
                e.NewElement.Unfocused += HandleEvent;
            }
        }

        void HandleEvent(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}