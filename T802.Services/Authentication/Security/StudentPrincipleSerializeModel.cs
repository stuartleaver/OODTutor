using System.Collections.Generic;
using T802.Core.Domain.Students;

namespace T802.Services.Authentication.Security
{
    public class StudentPrincipleSerializeModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public ICollection<StudentRole> Roles { get; set; } 
    }
}