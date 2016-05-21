using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace T802.Web.Framework
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _allowedRoles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _allowedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;
            if (httpContext.User.Identity.IsAuthenticated && !_allowedRoles.Any())
                return true;

            foreach (var role in _allowedRoles)
            {
                if (HttpContext.Current.User.IsInRole(role))
                {
                    authorize = true;
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var authUrl = ""; //passed from attribute

            //if null, get it from config
            if (String.IsNullOrEmpty(authUrl))
                authUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"];

            if (!String.IsNullOrEmpty(authUrl))
                filterContext.HttpContext.Response.Redirect(authUrl);

            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}