using System.Collections.Generic;

namespace ArxivExpress.Features.SearchArticles
{
    public interface IArticleEntry
    {
        public List<string> Categories
        {
            get;
        }

        public List<Contributor> Contributors
        {
            get;
        }

        public string PdfUrl
        {
            get;
        }

        public string Summary
        {
            get;
        }

        public string Title
        {
            get;
        }

        public string Published
        {
            get;
        }

        public string LastUpdated
        {
            get;
        }

        public string Id
        {
            get;
        }
    }
}
