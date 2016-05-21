using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T802.Core.Domain.Students
{
    public class RewardPointsHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the student identifier
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the points redeemed/added
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the points balance
        /// </summary>
        public int PointsBalance { get; set; }

        /// <summary>
        /// Gets or sets the used amount
        /// </summary>
        public decimal UsedAmount { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the order for which points were redeemed as a payment
        /// </summary>
        //public virtual Order UsedWith { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Student Student { get; set; }
    }
}
