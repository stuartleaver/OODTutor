using System.Collections;
using System.Collections.Generic;

namespace T802.Core.Domain.Students
{
    public partial class Achievement : BaseEntity
    {
        private ICollection<AchievementHistory> _achievemehtHistory;

        /// <summary>
        /// Gets or sets the achievement name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the achievement is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the achievement system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the achievement description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the achievement level
        /// </summary>
        public int AchievementLevelId { get; set; }

        #region Navigation properties

        /// <summary>
        /// Gets or sets the achievement history
        /// </summary>
        public virtual ICollection<AchievementHistory> AchievementHistory
        {
            get { return _achievemehtHistory ?? (_achievemehtHistory = new List<AchievementHistory>()); }
            protected set { _achievemehtHistory = value; }
        }

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual AchievementLevel AchievementLevel { get; set; }

        #endregion
    }
}
