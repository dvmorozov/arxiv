﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ArxivExpress.Features.Data;

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

        private List<Article> LoadArticlesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);

                return LoadArticlesFromRoot(xml.Root);
            }

            return new List<Article>();
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
