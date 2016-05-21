using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace T802.Web.Framework
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Render CSS styles of selected index 
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="currentIndex">Current tab index (where appropriate CSS style should be rendred)</param>
        /// <param name="indexToSelect">Tab index to select</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString RenderSelectedTabIndex(this HtmlHelper helper, int currentIndex, int indexToSelect)
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            //ensure it's not negative
            if (indexToSelect < 0)
                indexToSelect = 0;

            //required validation
            if (indexToSelect == currentIndex)
            {
                return new MvcHtmlString(" class='k-state-active'");
            }

            return new MvcHtmlString("");
        }

        /// <summary>
        /// Render a credit link
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="url">Page URL</param>
        /// <param name="display">Display name</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString CreditLink(this HtmlHelper html, string url, string display)
        {
            var link = String.Format("Credit - <a href=\"{0}\">{1}</a>", url, display);
            return new MvcHtmlString(link);
        }


        /// <summary>
        /// Render a block quote
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="text">Quote text</param>
        /// <param name="source">Source</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString Blockquote(this HtmlHelper html, string text, string source)
        {
            var blockquote = new TagBuilder("blockquote");

            var quote = new TagBuilder("p");
            quote.InnerHtml = text;

            var small = new TagBuilder("small");
            var cite = new TagBuilder("cite");
            cite.InnerHtml = source;
            small.InnerHtml = "by " + cite;

            blockquote.InnerHtml += quote;
            blockquote.InnerHtml += small;

            return new MvcHtmlString(blockquote.ToString());
        }

        /// <summary>
        /// Renders an image tag
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="src">Source</param>
        /// <param name="altText">Alternate text</param>
        /// <param name="height">Image height</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}