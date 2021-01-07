using System;
using System.Collections.Generic;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticlesRepository
    {
        public LikedArticlesRepository()
        {
            _articleIds = new List<string>();
        }

        private List<string> _articleIds;

        public void AddArticle(string id)
        {
            _articleIds.Add(id);
        }

        public void DeleteArticle(string id)
        {
            if (_articleIds.Contains(id))
            {
                _articleIds.Remove(id);
            }
        }

        public bool HasArticle(string id)
        {
            return _articleIds.Contains(id);
        }
    }
}
