using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Infrastructure
{
    ///<summary>Provides url standards across the application</summary>
    public static class UrlHelper
    {
        public static string GetUrlTitleFromPageTitle(string pageTitle)
        {
            string _urlTitle = "";
            if (pageTitle == null) return "";

            foreach (var letter in pageTitle)
            {
                if (char.IsLetterOrDigit(letter))
                {
                    _urlTitle += letter;
                }
                else if (letter == ' ')
                {
                    _urlTitle += "-";
                }
            }
            return _urlTitle;
        }

    }
}
