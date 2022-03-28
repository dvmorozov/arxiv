// ****************************************************************************
//    File "Contributor.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

namespace ArxivExpress.Features.SearchArticles
{
    public class Contributor
    {
        private string _name;
        private string _email;

        public string Email { get { return _email ?? "unknown"; } }
        public string Name { get { return _name ?? "unknown"; } }

        public Contributor(string name, string email)
        {
            _name = name;
            _email = email;
        }
    }
}
