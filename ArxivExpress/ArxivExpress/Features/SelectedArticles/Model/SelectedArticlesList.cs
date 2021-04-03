namespace ArxivExpress.Features.SelectedArticles.Model
{
    public class SelectedArticlesList
    {
        public SelectedArticlesList()
        {
        }

        private string _name;

        public string Name
        {
            get
            {
                return _name ?? "unknown";
            }

            set
            {
                _name = value;
            }
        }
    }
}
