using System;
using System.Xml.Linq;
using ArxivExpress.Features.Data;

namespace ArxivExpress.Features.SelectedArticles.Data
{
    public class SelectedArticlesListRepository : ArticlesRepository
    {
        private readonly XElement _root;

        public SelectedArticlesListRepository(XElement root)
        {
            _root = root;
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

            var articleListElement = GetArticlesRoot();
            //foreach (var articleElement in articleListElement.)
            //{
            //}
        }
    }
}
