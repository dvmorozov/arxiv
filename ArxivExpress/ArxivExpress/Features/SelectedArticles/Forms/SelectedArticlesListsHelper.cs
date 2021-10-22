using System;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using Xamarin.Forms;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class SelectedArticlesListsHelper
    {
        private INavigation _navigation;
        private SelectedArticlesListsRepository _selectedArticlesListsRepository;

        public SelectedArticlesListsRepository Repository => _selectedArticlesListsRepository;

        public SelectedArticlesListsHelper(INavigation navigation)
        {
            _navigation = navigation ??
                throw new Exception("Navigation is not assigned.");

            _selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
        }

        public async Task OpenArticleList(string listName)
        {
            var rootNode = _selectedArticlesListsRepository.GetArticleListRoot(listName);

            var selectedArticlesListRepository = new SelectedArticlesListRepository(rootNode);
            var articleList = new SelectedArticleList(
                selectedArticlesListRepository, _selectedArticlesListsRepository, rootNode.Attribute("Name")?.Value);

            await _navigation.PushAsync(articleList);
        }

        public async Task AddArticleToList(string listName, IArticleEntry articleEntry)
        {
            var rootNode = _selectedArticlesListsRepository.GetArticleListRoot(listName);

            var articlesListRepository = new SelectedArticlesListRepository(rootNode);
            articlesListRepository.AddArticle(articleEntry);

            _selectedArticlesListsRepository.ReplaceArticleListElement(articlesListRepository.Root);

            await OpenArticleList(listName);
        }
    }
}
