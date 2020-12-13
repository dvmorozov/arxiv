using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void Handle_ReadingData(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList());
        }
    }
}
