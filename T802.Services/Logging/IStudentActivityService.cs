using System;
using System.Collections.Generic;
using T802.Core;
using T802.Core.Domain.Logging;
using T802.Core.Domain.Students;

namespace T802.Services.Logging
{
    /// <summary>
    /// Customer activity service interface
    /// </summary>
    public partial interface IStudentActivityService
    {   
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        IList<ActivityLogType> GetAllActivityTypes();
        
        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(string systemKeyword, string comment, params object[] commentParams);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="customer">The customer</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(string systemKeyword, 
            string comment, Student customer, params object[] commentParams);
    }
}
