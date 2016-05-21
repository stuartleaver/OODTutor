using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using T802.Core;
using T802.Core.Domain.Students;
using T802.Core.Caching;
using T802.Services.Authentication;

namespace T802.Web.Framework
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string StudentCookieName = "T802.Student";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IAuthenticationService _authenticationService;

        private Student _cachedStudent;

        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IAuthenticationService authenticationService)
        {
            this._httpContext = httpContext;
            this._authenticationService = authenticationService;
        }

        #endregion

        #region Utilities

        protected virtual void SetStudentCookie(Guid studentGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(StudentCookieName);
                cookie.HttpOnly = true;
                cookie.Value = studentGuid.ToString();
                if (studentGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(StudentCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current student
        /// </summary>
        public virtual Student CurrentStudent
        {
            get
            {
                if (_cachedStudent != null)
                    return _cachedStudent;

                Student student = _authenticationService.GetAuthenticatedStudent();
                
                //validation
                if (student != null && !student.Deleted && student.Active)
                {
                    SetStudentCookie(student.StudentGuid);
                    _cachedStudent = student;
                }

                return _cachedStudent;
            }
            set
            {
                SetStudentCookie(value.StudentGuid);
                _cachedStudent = value;
            }
        }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
