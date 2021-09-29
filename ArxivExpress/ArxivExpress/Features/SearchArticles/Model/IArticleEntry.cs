﻿using System.Collections.Generic;

namespace ArxivExpress.Features.SearchArticles
{
    public interface IArticleEntry
    {
        public abstract List<string> Categories
        {
            get;
        }

        public abstract List<Contributor> Contributors
        {
            get;
        }

        public abstract string ContributorsAbbreviated
        {
            get;
        }

        public abstract string PdfUrl
        {
            get;
        }

        public abstract string Summary
        {
            get;
        }

        public abstract string Title
        {
            get;
        }

        public abstract string Published
        {
            get;
        }

        public abstract string PublishedWithLabel
        {
            get;
        }

        public abstract string LastUpdated
        {
            get;
        }

        public abstract string LastUpdatedWithLabel
        {
            get;
        }

        public abstract string Id
        {
            get;
        }
    }
}
