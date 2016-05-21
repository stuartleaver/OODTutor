using T802.Core.Domain.Students;

namespace T802.Services.Students
{
    public interface IStudentRegistrationService
    {
        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="usernameOrEmail">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        StudentLoginResults ValidateStudent(string username, string password);

        /// <summary>
        /// Register student
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        StudentRegistrationResult RegisterStudent(StudentRegistrationRequest request);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        PasswordChangeResult ChangePassword(ChangePasswordRequest request);
    }
}
