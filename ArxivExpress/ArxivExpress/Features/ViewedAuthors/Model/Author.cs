// ****************************************************************************
//    File "Author.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

namespace ArxivExpress.Features.ViewedAuthors.Model
{
    public class Author
    {
        public Author()
        {
        }

        private string _name;

        public string Name
        {
            get
            {
                return _name ?? "unknown";
            }

            set
            {
                _name = value;
            }
        }
    }
}
