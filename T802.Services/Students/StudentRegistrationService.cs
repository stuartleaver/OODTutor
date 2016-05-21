using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Security;

namespace T802.Services.Students
{
    public class StudentRegistrationService : IStudentRegistrationService
    {
        private readonly IStudentService _studentService;
        private readonly IAchievementService _achievementService;
        private readonly IEncryptionService _encryptionService;

        public StudentRegistrationService(IStudentService studentService,
            IAchievementService achievementService,
            IEncryptionService encryptionService)
        {
            this._studentService = studentService;
            this._achievementService = achievementService;
            this._encryptionService = encryptionService;
        }

        /// <summary>
        /// Validate student
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual StudentLoginResults ValidateStudent(string username, string password)
        {
            Student student = _studentService.GetStudentByUsername(username);

            if (student == null)
                return StudentLoginResults.StudentNotExist;
            if (student.Deleted)
                return StudentLoginResults.Deleted;
            if (!student.Active)
                return StudentLoginResults.NotActive;
            //only registered can login
            if (!student.IsRegistered())
                return StudentLoginResults.NotRegistered;

            string pwd = "";
            pwd = _encryptionService.CreatePasswordHash(password, student.PasswordSalt);

            bool isValid = pwd == student.Password;
            if (!isValid)
                return StudentLoginResults.WrongPassword;

            //save last login date
            student.LastLoginDateUtc = DateTime.UtcNow;
            _studentService.UpdateStudent(student);
            return StudentLoginResults.Successful;
        }

        /// <summary>
        /// Register student
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual StudentRegistrationResult RegisterStudent(StudentRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new StudentRegistrationResult();
            if (request.Student.IsRegistered())
            {
                result.AddError("Current student is already registered");
                return result;
            }
            if (String.IsNullOrEmpty(request.Username))
            {
                result.AddError("Username Is Not Provided");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Password Is Not Provided");
                return result;
            }

            //validate unique user
            if (_studentService.GetStudentByUsername(request.Username) != null)
                {
                    result.AddError("Username Already Exists");
                    return result;
                }

            //at this point request is valid
            request.Student.Username = request.Username;

            string saltKey = _encryptionService.CreateSaltKey(5);
                        request.Student.PasswordSalt = saltKey;
                        request.Student.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey);


            request.Student.Active = request.IsApproved;

            //add to 'Registered' role

            var registeredRole = _studentService.GetAvailableStudentRole();
            if (registeredRole == null)
                throw new T802Exception("'Registered' role could not be loaded");
            request.Student.StudentRoles.Add(registeredRole);

            // Add rewards points
            request.Student.AddRewardPointsHistoryEntry(100, "Registered as student");

            // Add achievement
            request.Student.AddAchievementHistoryEntry(_achievementService.GetAchievementBySystemName(SystemStudentAchievementNames.Registered));

            _studentService.InsertStudent(request.Student);
            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual PasswordChangeResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new PasswordChangeResult();
            if (String.IsNullOrWhiteSpace(request.Username))
            {
                result.AddError("Username Is Not Provided");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("Password Is Not Provided");
                return result;
            }

            var student = _studentService.GetStudentByUsername(request.Username);
            if (student == null)
            {
                result.AddError("Email Not Found");
                return result;
            }


            var requestIsValid = false;
            if (request.ValidateRequest)
            {
                //password
                string oldPwd = _encryptionService.CreatePasswordHash(request.OldPassword, student.PasswordSalt);

                bool oldPasswordIsValid = oldPwd == student.Password;
                if (!oldPasswordIsValid)
                    result.AddError("Old Password Doesnt Match");

                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
                requestIsValid = true;


            //at this point request is valid
            if (requestIsValid)
            {
                string saltKey = _encryptionService.CreateSaltKey(5);
                student.PasswordSalt = saltKey;
                student.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey);

                _studentService.UpdateStudent(student);
            }

            return result;
        }
    }
}
