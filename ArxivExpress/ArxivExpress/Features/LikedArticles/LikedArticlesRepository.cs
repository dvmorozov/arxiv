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
        private static List<LikedArticle> _likedArticles;
        private const string _likedArticleListElementName = "LikedArticleList";
        private const string _likedArticleElementName = "LikedArticle";
        private const string _contributorListElementName = "ContributorList";
        private const string _contributorElementName = "Contributor";

        public List<LikedArticle> LikedArticles => _likedArticles;

        protected LikedArticlesRepository()
        {
        	_likedArticles = new List<LikedArticle>();
            LoadArticles();
        }

        protected virtual string GetFileName()
        {
            return "liked_articles.xml";
        }

        protected string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        protected List<LikedArticle> LoadArticlesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);

                return (
                    from likedArticle
                    in xml.Root.Descendants(_likedArticleElementName)
                    select new LikedArticle
                    {
                        Id = likedArticle.Attribute("Id").Value,

                        LastUpdated = likedArticle.Attribute("LastUpdated")?.Value,
                        Published = likedArticle.Attribute("Published")?.Value,
                        Title = likedArticle.Attribute("Title")?.Value,
                        Categories = likedArticle.Attribute("Categories")?.Value.Split(';').ToList(),
                        PdfUrl = likedArticle.Attribute("PdfUrl")?.Value,
                        Summary = likedArticle.Attribute("Summary")?.Value,

                        Contributors = (
                            from contributor
                            in likedArticle.Descendants(_contributorListElementName)
                            select new ArticleList.Contributor(
                                contributor.Attribute("Name")?.Value,
                                contributor.Attribute("Email")?.Value
                            )
                        ).ToList()
                    }
                ).ToList();
            }

            return new List<LikedArticle>();
        }

        protected virtual void LoadArticles()
        {
            _likedArticles.Clear();

            var filePath = GetFilePath();

            _likedArticles = LoadArticlesFromFile(filePath);
        }

        private XElement GetContributors(LikedArticle likedArticle)
        {
            var contributorElements = new XElement[0];

            if (likedArticle.Contributors != null)
            {
                contributorElements = new XElement[likedArticle.Contributors.Count];

                for (var i = 0; i < likedArticle.Contributors.Count; i++)
                {
                    var objects = new object[2];
                    objects[0] = new XAttribute("Name", likedArticle.Contributors[i].Name);
                    objects[1] = new XAttribute("Email", likedArticle.Contributors[i].Email);

                    contributorElements[i] = new XElement(_contributorElementName, objects);
                }
            }

            return new XElement(_contributorListElementName, contributorElements);
        }

        protected virtual void SaveArtcles()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var likedArticleElements = new XElement[_likedArticles.Count];

            for (var i = 0; i < _likedArticles.Count; i++)
            {
                var objects = new object[8];
                objects[0] = new XAttribute("Id", _likedArticles[i].Id);
                objects[1] = new XAttribute("LastUpdated", _likedArticles[i].LastUpdated);
                objects[2] = new XAttribute("Published", _likedArticles[i].Published);
                objects[3] = new XAttribute("Title", _likedArticles[i].Title);
                objects[4] = new XAttribute("Categories", string.Join(";", _likedArticles[i].Categories));
                objects[5] = new XAttribute("PdfUrl", _likedArticles[i].PdfUrl);
                objects[6] = new XAttribute("Summary", _likedArticles[i].Summary);
                objects[7] = GetContributors(_likedArticles[i]);

                likedArticleElements[i] = new XElement(_likedArticleElementName, objects);
            }
            xml.Add(new XElement(_likedArticleListElementName, likedArticleElements));
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

        public void AddArticle(LikedArticle likedArticle)
        {
            _likedArticles.Add(likedArticle);
            SaveArtcles();
        }

        public void DeleteArticle(string articleId)
        {
            if (_likedArticles.Exists(item => item.Id == articleId))
            {
                _likedArticles.RemoveAll(item => item.Id == articleId);
                SaveArtcles();
            }
        }

        public bool HasArticle(string articleId)
        {
            return _likedArticles.Exists(item => item.Id == articleId);
        }
    }
}
