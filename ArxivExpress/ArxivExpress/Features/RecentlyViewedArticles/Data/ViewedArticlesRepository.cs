using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.ViewedAuthors.Data;
using ArxivExpress.Features.ViewedAuthors.Model;

namespace ArxivExpress.Features.RecentlyViewedArticles.Data
{
    public class ViewedArticlesRepository : LikedArticlesRepository
    {
        private static ViewedArticlesRepository _instance;
        private uint _maxArticleNumber = 1000;

        private ViewedAuthorsRepository _viewedAuthorsRepository;

        protected ViewedArticlesRepository() : base()
        {
            _viewedAuthorsRepository = ViewedAuthorsRepository.GetInstance();
        }

        protected override string FileName => "viewed_articles.xml";

        protected override string ArticleListElementName => "ViewedArticleList";

        protected override string ArticleElementName => "ViewedArticle";

        public static new ViewedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ViewedArticlesRepository();
            }

            return _instance;
        }

        private void MoveArticleToTop(Article article)
        {
            if (_likedArticles.Exists(item => item.Id == article.Id))
            {
                _likedArticles.RemoveAll(item => item.Id == article.Id);
                _likedArticles.Insert(0, article);
            }
        }

        private void LimitArticleNumber()
        {
            while (_likedArticles.Count > _maxArticleNumber)
            {
                _likedArticles.RemoveAt(_likedArticles.Count - 1);
            }
        }

        private void AddAuthors(Article article)
        {
            foreach (var autor in article.Contributors)
            {
                _viewedAuthorsRepository.AddAuthor(
                    new Author() { Name = autor.Name });
            }
        }

        public override void AddArticle(Article article)
        {
            if (!_likedArticles.Exists(item => item.Id == article.Id))
            {
                _likedArticles.Insert(0, article);
                LimitArticleNumber();
                SaveArtcles();

                AddAuthors(article);
            }
            else
                MoveArticleToTop(article);
        }
    }
}
