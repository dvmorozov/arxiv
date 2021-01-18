using System.Collections.Generic;
using Microsoft.SyndicationFeed;

namespace ArxivExpress.Features.ArticleList
{
    public class ArticleEntry : IArticleEntry
    {
        private IAtomEntry _entry;

        public ArticleEntry(IAtomEntry entry)
        {
            _entry = entry;
        }

        public List<string> Categories
        {
            get
            {
                List<string> result = new List<string>();

                if (_entry.Categories != null)
                {
                    foreach (var category in _entry.Categories)
                    {
                        result.Add(category.Name);
                    }
                }

                return result;
            }
        }

        public List<Contributor> Contributors
        {
            get
            {
                List<Contributor> result = new List<Contributor>();

                if (_entry.Contributors != null)
                {
                    foreach (var contributor in _entry.Contributors)
                    {
                        result.Add(new Contributor(
                            contributor.Name, contributor.Email)
                        );
                    }
                }

                return result;
            }
        }

        public string PdfUrl
        {
            get
            {
                string result = null;

                if (_entry.Links != null)
                {
                    foreach (var link in _entry.Links)
                    {
                        if (link.MediaType == "application/pdf")
                        {
                            result = link.Uri.ToString();
                        }
                    }
                }

                return result;
            }
        }

        public string Summary
        {
            get { return MakePlainString(_entry.Summary); }
        }

        public string Title
        {
            get { return MakePlainString(_entry.Title); }
        }

        public string Published
        {
            get
            {
                return "Published: " +
                  _entry.Published != null ?
                  _entry.Published.Date.ToShortDateString() :
                  "unknown";
            }
        }

        public string LastUpdated
        {
            get
            {
                return "Last updated: " +
                  _entry.LastUpdated != null ?
                  _entry.LastUpdated.Date.ToShortDateString() :
                  "unknown";
            }
        }

        public string Id
        {
            get
            {
                return _entry.Id;
            }
        }

        private string MakePlainString(string original)
        {
            string result = original;

            if (result != null)
            {
                result = result.Trim(new char[] { ' ', '\t', '\n' });
                result = result.Replace('\n', ' ');
                result = result.Replace('\t', ' ');

                var length = result.Length;
                while (true)
                {
                    result = result.Replace("  ", " ");
                    if (result.Length < length)
                    {
                        length = result.Length;
                        continue;
                    }
                    break;
                }
            }
            else
                result = "unknown";

            return result;
        }
    }
}
