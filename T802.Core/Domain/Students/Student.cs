using System;
using System.Collections.Generic;
using T802.Core.Domain.Quizzes;

namespace T802.Core.Domain.Students
{
    public partial class Student : BaseEntity
    {
        private ICollection<StudentRole> _studentRoles;
        private ICollection<RewardPointsHistory> _rewardPointsHistory;
        private ICollection<LevelPointsHistory> _levelPointsHistory;
        private ICollection<AchievementHistory> _achievementHistory;
        private ICollection<Quiz> _studentQuizzes;

        /// <summary>
        /// Ctor
        /// </summary>
        public Student()
        {
            StudentGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the student Guid
        /// </summary>
        public Guid StudentGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        #region Navigation properties

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<StudentRole> StudentRoles
        {
            get { return _studentRoles ?? (_studentRoles = new List<StudentRole>()); }
            protected set { _studentRoles = value; }
        }

        /// <summary>
        /// Gets or sets reward points history
        /// </summary>
        public virtual ICollection<RewardPointsHistory> RewardPointsHistory
        {
            get { return _rewardPointsHistory ?? (_rewardPointsHistory = new List<RewardPointsHistory>()); }
            protected set { _rewardPointsHistory = value; }
        }

        /// <summary>
        /// Gets or sets achievement history
        /// </summary>
        public virtual ICollection<AchievementHistory> AchievementHistory
        {
            get { return _achievementHistory ?? (_achievementHistory = new List<AchievementHistory>()); }
            protected set { _achievementHistory = value; }
        }

        /// <summary>
        /// Gets or sets level points history
        /// </summary>
        public virtual ICollection<LevelPointsHistory> LevelPointsHistory
        {
            get { return _levelPointsHistory ?? (_levelPointsHistory = new List<LevelPointsHistory>()); }
            protected set { _levelPointsHistory = value; }
        }

        /// <summary>
        /// Gets or sets the quizzes
        /// </summary>
        public virtual ICollection<Quiz> StudentQuizzes
        {
            get { return _studentQuizzes ?? (_studentQuizzes = new List<Quiz>()); }
            protected set { _studentQuizzes = value; }
        }

        #endregion
    }
}
