using System;
using System.Linq;
using System.Web;
using T802.Core.Caching;
using T802.Core.Data;
using T802.Core.Domain.Students;
using T802.Core.Helpers;
using T802.ServiceBusMessaging.Achievements;

namespace T802.Services.Students
{
    public class StudentService : IStudentService
    {
        private const string STUDENTROLES_BY_SYSTEMNAME_KEY = "T802.studentrole.systemname-{0}";

        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<StudentRole> _studentRoleRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IAchievementMessagingService _achievementMessagingService;

        public StudentService(IRepository<Student> studentRepository,
            IRepository<StudentRole> studentRoleRepository,
            ICacheManager cacheManager,
            IAchievementMessagingService achievementMessagingService)
        {
            _studentRepository = studentRepository;
            _studentRoleRepository = studentRoleRepository;
            _cacheManager = cacheManager;
            _achievementMessagingService = achievementMessagingService;
        }

        /// <summary>
        /// Get student by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Student</returns>
        public virtual Student GetStudentByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _studentRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var student = query.FirstOrDefault();
            return student;
        }

        public virtual void InsertStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            _studentRepository.Insert(student);
        }

        public virtual void UpdateStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            _studentRepository.Update(student);

            if (HttpContext.Current.Session["AchievementAwarded"] != null)
                if (HttpContext.Current.Session["AchievementAwarded"].ToString() == "1")
                _achievementMessagingService.SendAchievementAwardedMessage(student.Username);
        }

        public virtual StudentRole GetStudentRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(STUDENTROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _studentRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var studentRole = query.FirstOrDefault();
                return studentRole;
            });
        }

        /// <summary>
        /// Gets the role that the student is to be assigned
        /// </summary>
        /// <returns>Student Role</returns>
        public StudentRole GetAvailableStudentRole()
        {
            var adminSystemRoleName = GetStudentRoleBySystemName(SystemStudentRoleNames.Administrators);

            var query = from c in _studentRepository.Table
                orderby c.Id
                where (c.StudentRoles.Any(r => r.SystemName != adminSystemRoleName.SystemName))
                select c;

            return GetStudentRoleBySystemName(!NumericHelper.IsOdd(query.Count()) ? SystemStudentRoleNames.Game : SystemStudentRoleNames.Traditional);
        }
    }
}
