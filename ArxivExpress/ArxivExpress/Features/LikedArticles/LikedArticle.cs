using System.Collections.Generic;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticle
    {
        public LikedArticle()
        {
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

        private List<ArticleList.Contributor> _contributors;

        public List<ArticleList.Contributor> Contributors
        {
            get
            {
                return _contributors ?? new List<ArticleList.Contributor>();
            }

            set
            {
                _contributors = value;
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
                return _published ?? "unknown";
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
                return _lastUpdated ?? "unknown";
            }

            set
            {
                _lastUpdated = value;
            }
        }

        public string Id;
    }
}
