// ****************************************************************************
//    File "Article.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System.Collections.Generic;
using ArxivExpress.Features.SearchArticles;

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

        public virtual List<string> Categories
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

        public virtual List<Contributor> Contributors
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

        public virtual string PdfUrl
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

        public virtual string Summary
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

        public virtual string Title
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

        public virtual string Published
        {
            get
            {
                return _published ?? "unknown";
            }

            set
            {
                _published = value;
            }
        }

        private string _lastUpdated;

        public virtual string LastUpdated
        {
            get
            {
                return _lastUpdated ?? "unknown";
            }

            set
            {
                _lastUpdated = value;
            }
        }

        private string _id;

        public virtual string Id
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

        public virtual string ContributorsAbbreviated
        {
            get
            {
                var contributors = Contributors;
                if (contributors.Count != 0)
                {
                    var result = contributors[0].Name;
                    if (contributors.Count > 1)
                        result += " et al.";

                    return result;
                }

                return "unknown";
            }
        }

        public virtual string PublishedWithLabel
        {
            get
            {
                return "Published: " + Published;
            }
        }

        public virtual string LastUpdatedWithLabel
        {
            get
            {
                return "Last updated: " + LastUpdated;
            }
        }
    }
}
