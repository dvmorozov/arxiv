// ****************************************************************************
//    File "ToggleLikeButton.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.LikedArticles.Forms
{
    public class ToggleLikeButton : StyledButton
    {
        public ToggleLikeButton(IArticleEntry articleEntry) : base("icons8_heart_32")
        {
            BindingContext = articleEntry;

            Clicked += Handle_Pressed;
            SetIcon();
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
            SetIcon();
        }

        private void SetIcon()
        {
            if (BindingContext is IArticleEntry articleEntry)
            {
                var likedArticlesRepository = LikedArticlesRepository.GetInstance();
                ImageSource = likedArticlesRepository.HasArticle(articleEntry.Id)
                    ? "icons8_love_32" : "icons8_heart_32";
            }
        }
    }
}
