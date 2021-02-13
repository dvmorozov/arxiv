using System;
using UIKit;

namespace ArxivExpress.iOS
{
    public class UIBarButtons
    {
        private UIBarButtonItem[] _uiBarButtons;

        public UIBarButtons()
        {
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
            });

            _uiBarButtons = new UIBarButtonItem[] {
                new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };
        }
    }
}
