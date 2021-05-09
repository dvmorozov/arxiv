using System;
using System.Collections.Generic;
using System.IO;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Data
{
    public class SelectedArticlesRepository : LikedArticlesRepository
    {
        private static SelectedArticlesRepository _instance;
        private XElement _root;

        protected SelectedArticlesRepository(XElement root)
        {
            _root = root;
            _likedArticles = LoadArticlesFromRoot(_root);
        }

        protected virtual string ArticleListElementName => "SelectedArticleList";

        protected virtual string ArticleElementName => "SelectedArticle";

        public static SelectedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SelectedArticlesRepository();
            }

            return _instance;
        }
    }
}
