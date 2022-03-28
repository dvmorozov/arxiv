// ****************************************************************************
//    File "Buttons.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

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
