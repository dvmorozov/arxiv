using Xamarin.Forms;

namespace ArxivExpress
{
    public class StyledButton : Button
    {
        public StyledButton()
        {
            BorderColor = Color.FromHex("#2196F3");
            BackgroundColor = Color.FromHex("#2196F3");
            BorderWidth = 2;
            Margin = 10;
            HeightRequest = 50;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            WidthRequest = 200;
            TextColor = Color.White;
        }
    }
}
