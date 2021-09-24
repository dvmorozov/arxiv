using System.Xml.Linq;
using ArxivExpress.Features.Data;

namespace ArxivExpress.Features.SelectedArticles.Data
{
    /// <summary>
	/// Represents list of selected articles.
	/// </summary>
    public class SelectedArticlesListRepository : ArticlesRepository
    {
        private XElement _root;

        public XElement Root { get { return _root; } }

        public SelectedArticlesListRepository(XElement root)
        {
            _root = root;
            _articles = LoadArticlesFromRoot(_root);
        }

        protected override string ArticleElementName => "SelectedArticle";

        protected override string ArticleListElementName => "SelectedArticleList";

        protected override void SaveArticles()
        {
            var newRoot = GetArticlesRoot();

            //  Saves root attributes.
            _root.RemoveNodes();
            foreach (var descendant in newRoot.Descendants())
            {
                _root.Add(descendant);
            }
        }
    }
}
