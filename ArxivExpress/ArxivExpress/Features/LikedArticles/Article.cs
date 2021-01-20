using System.Collections.Generic;
using ArxivExpress.Features.ArticleList;

namespace ArxivExpress.Features.LikedArticles
{
    public class Article : IArticleEntry
    {
        public Article()
        {
        }

        public Article(IArticleEntry articleEntry)
        {
            Id = articleEntry.Id;
            Categories = articleEntry.Categories;
            Contributors = articleEntry.Contributors;
            Title = articleEntry.Title;
            Published = articleEntry.Published;
            LastUpdated = articleEntry.LastUpdated;
            PdfUrl = articleEntry.PdfUrl;
            Summary = articleEntry.Summary;
        }

        private List<string> _categories;

        public List<string> Categories
        {
            get
            {
                return _categories ?? new List<string>();
            }

            set
            {
                _categories = value;
            }
        }

        private List<Contributor> _contributors;

        public List<Contributor> Contributors
        {
            get
            {
                return _contributors ?? new List<Contributor>();
            }

            set
            {
                _contributors = value;
            }
        }

        private string _pdfUrl;

        public string PdfUrl
        {
            get
            {
                return _pdfUrl ?? "unknown";
            }

            set
            {
                _pdfUrl = value;
            }
        }

        private string _summary;

        public string Summary
        {
            get
            {
                return _summary ?? "unknown";
            }

            set
            {
                _summary = value;
            }
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title ?? "unknown";
            }

            set
            {
                _title = value;
            }
        }

        private string _published;

        public string Published
        {
            get
            {
                return "Published: " +
                  _published ?? "unknown";
            }

            set
            {
                _published = value;
            }
        }

        private string _lastUpdated;

        public string LastUpdated
        {
            get
            {
                return "Last updated: " + _lastUpdated ?? "unknown";
            }

            set
            {
                _lastUpdated = value;
            }
        }

        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
    }
}
