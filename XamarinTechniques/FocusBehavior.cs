﻿using Xamarin.Forms;

namespace XamarinTechniques
{
    public class FocusBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.Focused += Entry_Focused;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.Focused -= Entry_Focused;
        }

        void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.BackgroundColor = (e.IsFocused) ? Color.Yellow : Color.Default;
        }
    }
}
