using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Students;

namespace T802.Services.Students
{
    public interface IStudentService
    {
        Student GetStudentByUsername(string username);
        void InsertStudent(Student customer);
        void UpdateStudent(Student customer);
        StudentRole GetStudentRoleBySystemName(string systemName);

        /// <summary>
        /// Gets the role that the student is to be assigned
        /// </summary>
        /// <returns>Student Role</returns>
        StudentRole GetAvailableStudentRole();
    }
}
