using System;
using System.Collections.Generic;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticlesRepository
    {
        private static LikedArticlesRepository _instance;
        private static List<string> _articleIds;

        protected LikedArticlesRepository()
        {
        	_articleIds = new List<string>();
        }

        public static LikedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LikedArticlesRepository();
            }

            return _instance;
        }

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
