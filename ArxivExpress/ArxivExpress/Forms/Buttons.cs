using Xamarin.Forms;

namespace ArxivExpress
{
    public class StyledButton : Button
    {
        public StyledButton(string imageSource)
        {
            BorderColor = Color.Black;
            BackgroundColor = Color.White;
            BorderWidth = 1;
            Margin = 10;
            HeightRequest = 50;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            WidthRequest = 50;
            TextColor = Color.Black;
            ImageSource = imageSource;
        }
    }
}
