// ****************************************************************************
//    File "ArticlesRepository.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.Data
{
    public abstract class ArticlesRepository : IListRepository<IArticleEntry>
    {
        protected List<IArticleEntry> _articles;

        public ArticlesRepository()
        {
        }

        protected abstract string ArticleElementName
        {
            get;
        }

        protected abstract string ArticleListElementName
        {
            get;
        }

        private string _contributorListElementName => "ContributorList";

        private string _contributorElementName => "Contributor";

        /// <summary>
        /// Loads article list from XML element.
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Article list</returns>
        protected List<IArticleEntry> LoadArticlesFromRoot(XElement root)
        {
            var result = new List<IArticleEntry>();
            foreach (var articleElement in root.Descendants(ArticleElementName))
            {
                //  Attributes are case sensitive.
                if (articleElement.Attribute("Id") != null)
                {
                    var article = new Article
                    {
                        Id = articleElement.Attribute("Id").Value,
                        LastUpdated = articleElement.Attribute("LastUpdated")?.Value,
                        Published = articleElement.Attribute("Published")?.Value,
                        Title = articleElement.Attribute("Title")?.Value,
                        Categories = articleElement.Attribute("Categories")?.Value.Split(';').ToList(),
                        PdfUrl = articleElement.Attribute("PdfUrl")?.Value,
                        Summary = articleElement.Attribute("Summary")?.Value,
                    };

                    var contributors = new List<Contributor>();
                    foreach (var contributorListElement in articleElement.Descendants(_contributorListElementName))
                    {
                        foreach (var contributorElement in contributorListElement.Descendants(_contributorElementName))
                        {
                            var contributor = new Contributor(
                                contributorElement.Attribute("Name")?.Value,
                                contributorElement.Attribute("Email")?.Value
                                );
                            contributors.Add(contributor);
                        }
                    }

                    article.Contributors = contributors;
                    result.Add(article);
                }
            }

            return result;
        }

        private XElement GetContributors(IArticleEntry article)
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

        /// <summary>
        /// Returns article list as XML element.
        /// </summary>
        /// <returns>XML element</returns>
        protected XElement GetArticlesRoot()
        {
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

                articleElements[i] = new XElement(ArticleElementName, objects);
            }

            return new XElement(ArticleListElementName, articleElements);
        }

        public async Task<ObservableCollection<IArticleEntry>> LoadFirstPage()
        {
            var task = new Task<ObservableCollection<IArticleEntry>>(
                () =>
                {
                    var result = new ObservableCollection<IArticleEntry>();
                    var start = GetPageNumber() * GetResultsPerPage();
                    var count = IsLastPage() ?
                        _articles.Count - start : GetResultsPerPage();

                    foreach (var article in _articles.GetRange((int)start, (int)count))
                    {
                        result.Add(article);
                    }
                    return result;
                });

            task.Start();
            return await task;
        }

        public Task<ObservableCollection<IArticleEntry>> LoadNextPage()
        {
            if ((GetPageNumber() + 1) * GetResultsPerPage() < _articles.Count)
            {
                _pageNumber++;
            }
            return LoadFirstPage();
        }

        public Task<ObservableCollection<IArticleEntry>> LoadPrevPage()
        {
            if (_pageNumber > 0)
            {
                _pageNumber--;
            }
            return LoadFirstPage();
        }

        private uint _pageNumber = 0;

        public uint GetPageNumber()
        {
            return _pageNumber;
        }

        public uint GetResultsPerPage()
        {
            return 25;
        }

        public bool IsLastPage()
        {
            var start = GetPageNumber() * GetResultsPerPage();
            return GetResultsPerPage() >= _articles.Count - start;
        }

        public bool IsEmpty()
        {
            return _articles.Count == 0;
        }

        protected abstract void SaveArticles();

        public virtual void AddArticle(IArticleEntry article)
        {
            if (!_articles.Exists(item => item.Id == article.Id))
            {
                _articles.Add(article);
                SaveArticles();
            }
        }

        public void DeleteArticle(string articleId)
        {
            if (_articles.Exists(item => item.Id == articleId))
            {
                _articles.RemoveAll(item => item.Id == articleId);
                SaveArticles();
            }
        }

        public bool HasArticle(string articleId)
        {
            return _articles.Exists(item => item.Id == articleId);
        }
    }
}
