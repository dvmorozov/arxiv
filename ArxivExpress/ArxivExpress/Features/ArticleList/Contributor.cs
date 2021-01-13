namespace ArxivExpress.Features.ArticleList
{
    public class Contributor
    {
        private string _name;
        private string _email;

        public string Email { get { return _email ?? "unknown"; } }
        public string Name { get { return _name ?? "unknown"; } }

        public Contributor(string name, string email)
        {
            _name = name;
            _email = email;
        }
    }
}
