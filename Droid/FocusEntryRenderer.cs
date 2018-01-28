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
                e.OldElement.Focused -= Entry_Focused;
            }

            if (e.NewElement != null)
            {
                e.NewElement.Focused += Entry_Focused;
            }
        }

        void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}