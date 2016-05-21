using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Domain.Students
{
    public class AchievementHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the student identifier
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime AwardedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the achievement identifier
        /// </summary>
        public int AchievementId { get; set; }

        /// <summary>
        /// Gets or sets the student
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Gets or sets the Achievement
        /// </summary>
        public virtual Achievement Achievement { get; set; }
    }
}
