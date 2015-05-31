using System;
using System.Web.Mvc;
using System.Text;

namespace CounterPointPracticalTest.Extensions
{
    public static class PagerHelper
    {
        public static string Paged(this HtmlHelper helper, int curPage, int numbOfPages, int startPage, Func<int, string> url)
        {
            int pageFrom = (curPage <= startPage + 5 ? startPage: startPage + 5);
            var pagerStr = new StringBuilder();
            //Add Previous two Links:

            for (int i = pageFrom; i <= pageFrom + 4; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", url(i));
                tag.InnerHtml = i.ToString();
                if (i == curPage)
                {
                    tag.AddCssClass("selected");
                }
                pagerStr.AppendLine(tag.ToString());
            }
            //Add Next two Links:

            return pagerStr.ToString();
        }
    }
}
