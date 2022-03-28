// ****************************************************************************
//    File "SelectedArticlesList.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System.Xml.Linq;

namespace ArxivExpress.Features.SelectedArticles.Model
{
    public class SelectedArticlesList
    {
        public SelectedArticlesList(string tag, string name)
        {
            var attributes = new object[1];
            attributes[0] = new XAttribute("Name", name);
            InnerElement = new XElement(tag, attributes);
        }

        public SelectedArticlesList(XElement innerElement)
        {
            InnerElement = innerElement;
        }

        public XElement InnerElement { get; }
        public string Name { get { return InnerElement.Attribute("Name").Value; } }
    }
}
