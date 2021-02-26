using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ArxivExpress.Features.ViewedAuthors.Data
{
    public class ViewedAuthorsRepository
    {
        private static ViewedAuthorsRepository _instance;

        private ViewedAuthorsRepository()
        {
        }

        public static ViewedAuthorsRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ViewedAuthorsRepository();
            }

            return _instance;
        }

        private string GetFileName()
        {
            return "viewed_authors.xml";
        }

        private string GetElementName()
        {
            return "ViewedAuthors";
        }

        private string GetRootElementName()
        {
            return "ViewedAuthorsList";
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        public List<string> LoadAuthors()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var list = (
                    from searchQuery
                    in xml.Root.Descendants(GetElementName())
                    where searchQuery.Attribute("Name") != null
                    select searchQuery.Attribute("Name").Value 
                ).ToList();

                return list;
            }

            return new List<string>();
        }

        public void SaveAuthors(List<string> authors)
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var authorElements = new XElement[authors.Count];

            for (var i = 0; i < authors.Count; i++)
            {
                var attributes = new object[1];
                attributes[0] = new XAttribute("Name", authors[i]);

                authorElements[i] = new XElement(GetElementName(), attributes);
            }

            xml.Add(new XElement(GetRootElementName(), authorElements));
            xml.Save(filePath);
        }
    }
}
