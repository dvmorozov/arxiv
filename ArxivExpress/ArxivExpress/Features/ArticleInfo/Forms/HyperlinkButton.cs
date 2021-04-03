using System;
using Xamarin.Essentials;

namespace ArxivExpress.Features.ArticleInfo.Forms
{
    public class HyperlinkButton : StyledButton
    {
        public string Url { get; }

        public HyperlinkButton(string text, string url)
        {
            Url = url;
            Text = text;

            Clicked += new EventHandler(delegate (Object o, EventArgs a)
            {
                Launcher.OpenAsync(Url);
            });
        }
    }
}
