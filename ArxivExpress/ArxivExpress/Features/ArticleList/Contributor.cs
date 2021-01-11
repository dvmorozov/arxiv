namespace ArxivExpress.Features.ArticleList
{
    public class Contributor
    {
        public string Email { get; }
        public string Name { get; }

        public Contributor(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
