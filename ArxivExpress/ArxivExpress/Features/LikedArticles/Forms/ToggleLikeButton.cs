using System;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.LikedArticles.Forms
{
    public class ToggleLikeButton : StyledButton
    {
        public ToggleLikeButton(IArticleEntry articleEntry)
        {
            BindingContext = articleEntry;

            Clicked += Handle_Pressed;
            SetText();
        }

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                ToggleLikeStatus();
            }
        }

        private void ToggleLikeStatus()
        {
            var likedArticlesRepository = LikedArticlesRepository.GetInstance();

            if (BindingContext is IArticleEntry articleEntry)
            {
                if (likedArticlesRepository.HasArticle(articleEntry.Id))
                {
                    likedArticlesRepository.DeleteArticle(articleEntry.Id);
                }
                else
                {
                    likedArticlesRepository.AddArticle(new Article(articleEntry));
                }
            }
            SetText();
        }

        private void SetText()
        {
            if (BindingContext is IArticleEntry articleEntry)
            {
                var likedArticlesRepository = LikedArticlesRepository.GetInstance();
                Text = likedArticlesRepository.HasArticle(articleEntry.Id)
                    ? "Remove from Liked" : "Add to Liked";
            }
        }
    }
}
