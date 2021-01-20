using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticlesRepository
    {
        private static LikedArticlesRepository _instance;

        private List<Article> _articles;
        public List<Article> Articles => _articles;

        protected LikedArticlesRepository()
        {
        	_articles = new List<Article>();
            LoadArticles();
        }

        protected virtual string GetFileName()
        {
            return "liked_articles.xml";
        }

        protected virtual string GetArticleListElementName()
        {
            return "LikedArticleList";
        }

        protected virtual string GetArticleElementName()
        {
            return "LikedArticle";
        }

        private const string _contributorListElementName = "ContributorList";
        private const string _contributorElementName = "Contributor";

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        private List<Article> LoadArticlesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);

                return (
                    from article
                    in xml.Root.Descendants(GetArticleElementName())
                    select new Article
                    {
                        Id = article.Attribute("Id").Value,

                        LastUpdated = article.Attribute("LastUpdated")?.Value,
                        Published = article.Attribute("Published")?.Value,
                        Title = article.Attribute("Title")?.Value,
                        Categories = article.Attribute("Categories")?.Value.Split(';').ToList(),
                        PdfUrl = article.Attribute("PdfUrl")?.Value,
                        Summary = article.Attribute("Summary")?.Value,

                        Contributors = (
                            from contributor
                            in article.Descendants(_contributorListElementName)
                            select new ArticleList.Contributor(
                                contributor.Attribute("Name")?.Value,
                                contributor.Attribute("Email")?.Value
                            )
                        ).ToList()
                    }
                ).ToList();
            }

            return new List<Article>();
        }

        protected virtual void LoadArticles()
        {
            _articles.Clear();

            var filePath = GetFilePath();

            _articles = LoadArticlesFromFile(filePath);
        }

        private XElement GetContributors(Article article)
        {
            var contributorElements = new XElement[0];

            if (article.Contributors != null)
            {
                contributorElements = new XElement[article.Contributors.Count];

                for (var i = 0; i < article.Contributors.Count; i++)
                {
                    var objects = new object[2];
                    objects[0] = new XAttribute("Name", article.Contributors[i].Name);
                    objects[1] = new XAttribute("Email", article.Contributors[i].Email);

                    contributorElements[i] = new XElement(_contributorElementName, objects);
                }
            }

            return new XElement(_contributorListElementName, contributorElements);
        }

        protected void SaveArtcles()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var articleElements = new XElement[_articles.Count];

            for (var i = 0; i < _articles.Count; i++)
            {
                var objects = new object[8];
                objects[0] = new XAttribute("Id", _articles[i].Id);
                objects[1] = new XAttribute("LastUpdated", _articles[i].LastUpdated);
                objects[2] = new XAttribute("Published", _articles[i].Published);
                objects[3] = new XAttribute("Title", _articles[i].Title);
                objects[4] = new XAttribute("Categories", string.Join(";", _articles[i].Categories));
                objects[5] = new XAttribute("PdfUrl", _articles[i].PdfUrl);
                objects[6] = new XAttribute("Summary", _articles[i].Summary);
                objects[7] = GetContributors(_articles[i]);

                articleElements[i] = new XElement(GetArticleListElementName(), objects);
            }

            xml.Add(new XElement(GetArticleElementName(), articleElements));
            xml.Save(filePath);
        }

        public static LikedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LikedArticlesRepository();
            }

            return _instance;
        }

        public virtual void AddArticle(Article article)
        {
            _articles.Add(article);
            SaveArtcles();
        }

        public void DeleteArticle(string articleId)
        {
            if (_articles.Exists(item => item.Id == articleId))
            {
                _articles.RemoveAll(item => item.Id == articleId);
                SaveArtcles();
            }
        }

        public bool HasArticle(string articleId)
        {
            return _articles.Exists(item => item.Id == articleId);
        }
    }
}
