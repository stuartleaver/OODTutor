using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Students;

namespace T802.Services.Students
{
    public class StudentRegistrationRequest
    {
        public Student Student { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsApproved { get; set; }

        public StudentRegistrationRequest(Student student, string username,
            string password, 
            bool isApproved = true)
        {
            this.Student = student;
            this.Username = username;
            this.Password = password;
            this.IsApproved = isApproved;
        }
    }
}
