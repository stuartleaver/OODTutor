using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T802.Core;

namespace T802.Web.Framework
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private IWorkContext _workContext;

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
        }

        /// <summary>
        /// Gets a selected tab index (used in admin area to store selected tab index)
        /// </summary>
        /// <returns>Index</returns>
        public int GetSelectedTabIndex()
        {
            int index = 0;
            string dataKey = "nop.selected-tab-index";
            if (ViewData[dataKey] is int)
            {
                index = (int)ViewData[dataKey];
            }
            if (TempData[dataKey] is int)
            {
                index = (int)TempData[dataKey];
            }

            //ensure it's not negative
            if (index < 0)
                index = 0;

            return index;
        }

        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}