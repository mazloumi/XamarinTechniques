using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinTechniques;
using XamarinTechniques.Droid;

[assembly: ExportRenderer(typeof(FocusEntry), typeof(FocusEntryRenderer))]

namespace XamarinTechniques.Droid
{
    public class FocusEntryRenderer : EntryRenderer
    {
        public FocusEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

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