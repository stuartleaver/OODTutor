using System;
using System.Linq;
using T802.Core.Domain.Students;

namespace T802.Services.Students
{
    public class StudentExtensions
    {
        /// <summary>
        /// Gets a value indicating whether student is registered
        /// </summary>
        /// <param name="student">Student</param>
        /// <param name="onlyActiveStudentRoles">A value indicating whether we should look only in active student roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(Student student, bool onlyActiveStudentRoles = true)
        {
            if (IsInStudentRole(student, SystemStudentRoleNames.Administrators, onlyActiveStudentRoles) ||
                IsInStudentRole(student, SystemStudentRoleNames.Game, onlyActiveStudentRoles) ||
                IsInStudentRole(student, SystemStudentRoleNames.Traditional, onlyActiveStudentRoles))
            {
                return true;
            }
            return false;
        }

        public static bool IsInStudentRole(Student student,
            string studentRoleSystemName, bool onlyActiveStudentRoles = true)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            if (String.IsNullOrEmpty(studentRoleSystemName))
                throw new ArgumentNullException("studentRoleSystemName");

            var result = student.StudentRoles
                .FirstOrDefault(cr => (!onlyActiveStudentRoles || cr.Active) && (cr.SystemName == studentRoleSystemName)) != null;
            return result;
        }
    }
}
