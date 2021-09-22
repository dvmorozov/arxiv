using System;
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

        public SelectedArticlesListRepository(XElement root)
        {
            _root = root;

            LoadArticlesFromRoot(_root);
        }

        protected override string ArticleElementName => "SelectedArticle";

        protected override string ArticleListElementName => "SelectedArticleList";

        private void CleanRoot()
        {
            _root.RemoveAll();
        }

        protected override void SaveArticles()
        {
            CleanRoot();
            _root.AddFirst(GetArticlesRoot());
        }
    }
}
