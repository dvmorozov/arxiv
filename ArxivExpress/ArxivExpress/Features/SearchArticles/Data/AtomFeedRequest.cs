// ****************************************************************************
//    File "AtomFeedRequest.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;

namespace ArxivExpress.Features.SearchArticles
{
    public class AtomFeedRequest
    {
        public AtomFeedRequest()
        {
        }

        public interface IAtomFeedProcessor
        {
            void ProcessCategory(ISyndicationCategory category);
            void ProcessImage(ISyndicationImage image);
            void ProcessEntry(IAtomEntry entry);
            void ProcessLink(ISyndicationLink link);
            void ProcessPerson(ISyndicationPerson person);
            void ProcessContent(ISyndicationContent content);
        }

        public static async Task MakeRequest(string request, IAtomFeedProcessor processor)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            HttpWebRequest req = WebRequest.CreateHttp(request);
            req.Timeout = 30000;
            req.ContentType = "application/xml";
            req.Method = "GET";

            using WebResponse res = req.GetResponse();
            using Stream stream = res.GetResponseStream();
            await ReadAtomFeed(stream, processor);
        }

        private static async Task ReadAtomFeed(Stream stream, IAtomFeedProcessor processor)
        {
            using XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { Async = true });
            //
            // Create an AtomFeedReader
            var reader = new AtomFeedReader(xmlReader);

            //
            // Read the feed
            while (await reader.Read())
            {
                //
                // Check the type of the current element.
                switch (reader.ElementType)
                {
                    //
                    // Read category
                    case SyndicationElementType.Category:
                        ISyndicationCategory category = await reader.ReadCategory();
                        processor.ProcessCategory(category);
                        break;

                    //
                    // Read image
                    case SyndicationElementType.Image:
                        ISyndicationImage image = await reader.ReadImage();
                        processor.ProcessImage(image);
                        break;

                    //
                    // Read entry 
                    case SyndicationElementType.Item:
                        IAtomEntry entry = await reader.ReadEntry();
                        processor.ProcessEntry(entry);
                        break;

                    //
                    // Read link
                    case SyndicationElementType.Link:
                        ISyndicationLink link = await reader.ReadLink();
                        processor.ProcessLink(link);
                        break;

                    //
                    // Read person
                    case SyndicationElementType.Person:
                        ISyndicationPerson person = await reader.ReadPerson();
                        processor.ProcessPerson(person);
                        break;

                    //
                    // Read content
                    default:
                        ISyndicationContent content = await reader.ReadContent();
                        processor.ProcessContent(content);
                        break;
                }
            }
        }
    }
}
