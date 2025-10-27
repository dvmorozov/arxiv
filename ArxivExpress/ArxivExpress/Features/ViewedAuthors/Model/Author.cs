// ****************************************************************************
//    File "Author.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
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
