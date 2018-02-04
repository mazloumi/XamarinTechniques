using Xamarin.Forms;

namespace XamarinTechniques
{
    public class FocusTriggerAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry entry)
        {
            entry.BackgroundColor = (entry.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}
