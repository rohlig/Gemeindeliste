using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace LocalDatabaseSample.Behaviors
{
        public class MaskedBehavior : Behavior<Entry>
        {
            private string _mask = "";
            public string Mask
            {
                get => _mask;
                set
                {
                    _mask = value;
                }
            }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            
            var entry = sender as Entry;
            var text = Regex.Replace(entry.Text, "[^0-9.]", "");
            if(text.Length >= 2 && text[1] == '.')
            {
                text = "0" + text;
            }
            if (text.Length >= 5 && text[4] == '.')
            {
                text = text.Insert(3, "0");
            }
            if (text.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(Mask))

                    // 1. Adding the MaxLength
                    if (text.Length == _mask.Length)
                        entry.MaxLength = _mask.Length;

                // 2. Evaluating if the user is removing test
                if ((args.OldTextValue == null) || (args.OldTextValue.Length <= args.NewTextValue.Length))

                    // 3. Evaluating mask positions
                    for (int i = Mask.Length; i >= text.Length; i--)
                    {
                        if (Mask[(text.Length - 1)] != 'X')
                        {
                            text = text.Insert((text.Length - 1), Mask[(text.Length - 1)].ToString());
                        }
                    }
            }
            entry.Text = text.Replace("..",".");
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }
    }

}
