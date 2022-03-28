// ****************************************************************************
//    File "AtomFeedProcessor.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System.Collections.ObjectModel;
using Microsoft.SyndicationFeed;

namespace ArxivExpress.Features.SearchArticles
{
    public class AtomFeedProcessor : AtomFeedRequest.IAtomFeedProcessor
    {
        public ObservableCollection<ArticleEntry> Items { get; }

        public AtomFeedProcessor()
        {
            Items = new ObservableCollection<ArticleEntry>();
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessCategory(ISyndicationCategory category)
        {
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessImage(ISyndicationImage image)
        {
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessEntry(IAtomEntry entry)
        {
            if (entry != null)
                Items.Add(new ArticleEntry(entry));
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessLink(ISyndicationLink link)
        {
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessPerson(ISyndicationPerson person)
        {
        }

        void AtomFeedRequest.IAtomFeedProcessor.ProcessContent(ISyndicationContent content)
        {
        }
    }
}
