// ****************************************************************************
//    File "LikedArticlesRepository.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticlesRepository : ArticlesRepository
    {
        private static LikedArticlesRepository _instance;

        protected LikedArticlesRepository()
        {
            var filePath = GetFilePath();

            _articles = LoadArticlesFromFile(filePath);
        }

        protected virtual string FileName => "liked_articles.xml";

        protected override string ArticleListElementName => "LikedArticleList";

        protected override string ArticleElementName => "LikedArticle";

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), FileName);
        }

        private List<IArticleEntry> LoadArticlesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);

                return LoadArticlesFromRoot(xml.Root);
            }

            return new List<IArticleEntry>();
        }

        protected override void SaveArticles()
        {
            var xml = new XDocument();
            xml.Add(GetArticlesRoot());
            xml.Save(GetFilePath());
        }

        public static LikedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LikedArticlesRepository();
            }

            return _instance;
        }
    }
}
