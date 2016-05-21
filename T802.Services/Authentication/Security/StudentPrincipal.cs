using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using T802.Services.Students;

namespace T802.Services.Authentication.Security
{
    public class StudentPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Deserialize<StudentPrincipleSerializeModel>(ticket.UserData);

            return userData.Roles.ToList().Exists(r => r.SystemName == role);
        }

        public StudentPrincipal(string name)
        {
            this.Identity = new GenericIdentity(name);
        }

        public int Id { get; set; }
        public string Username { get; set; }
    }
}
