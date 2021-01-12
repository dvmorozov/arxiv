using System.Collections.Generic;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticle
    {
        public LikedArticle()
        {
        }

        public List<string> Categories;

        public List<ArticleList.Contributor> Contributors;

        public string Title;

        public string Published;

        public string LastUpdated;

        public string Id;
    }
}
