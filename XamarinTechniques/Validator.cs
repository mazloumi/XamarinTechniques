using Xamarin.Forms;

namespace XamarinTechniques
{
    public class Validator
    {
        public static readonly BindableProperty CheckFocusProperty =
                BindableProperty.CreateAttached("CheckFocus", typeof(bool), typeof(Validator), false, propertyChanged: CheckFocusPropertyChanged);

        public static bool GetCheckFocus(BindableObject view)
        {
            return (bool)view.GetValue(CheckFocusProperty);
        }

        public static void SetCheckFocus(BindableObject view, bool value)
        {
            view.SetValue(CheckFocusProperty, value);
        }

        private static void CheckFocusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null) return;

            if (GetCheckFocus(view))
            {
                view.Focused += View_Focused;
            } else {
                view.Focused -= View_Focused;
            }
        }

        static void View_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}