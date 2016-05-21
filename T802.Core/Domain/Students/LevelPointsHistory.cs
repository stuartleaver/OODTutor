using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Quizzes;

namespace T802.Core.Domain.Students
{
    public class LevelPointsHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the student identifier
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the quiz identifier
        /// </summary>
        public int QuizId { get; set; }

        /// <summary>
        /// Gets or sets the points redeemed/added
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Quiz Quiz { get; set; }
    }
}
