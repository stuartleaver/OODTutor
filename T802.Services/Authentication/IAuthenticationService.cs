using T802.Core.Domain.Students;

namespace T802.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        void SignIn(Student student, bool createPersistentCookie);
        void SignOut();
        Student GetAuthenticatedStudent();
    }
}