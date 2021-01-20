using ArxivExpress.Features.LikedArticles;

namespace ArxivExpress.Features.RecentlyViewedArticles
{
    public class ViewedArticlesRepository : LikedArticles.LikedArticlesRepository
    {
        private static ViewedArticlesRepository _instance;

        protected ViewedArticlesRepository()
        {
        }

        protected override string GetFileName()
        {
            return "viewed_articles.xml";
        }

        protected override string GetArticleListElementName()
        {
            return "ViewedArticleList";
        }

        protected override string GetArticleElementName()
        {
            return "ViewedArticle";
        }

        public static new ViewedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ViewedArticlesRepository();
            }

            return _instance;
        }

        public override void AddArticle(Article article)
        {
            _articles.Add(article);
            SaveArtcles();
        }
    }
}
