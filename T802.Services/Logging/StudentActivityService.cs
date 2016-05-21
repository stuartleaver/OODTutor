using System;
using System.Collections.Generic;
using System.Linq;
using T802.Core;
using T802.Core.Caching;
using T802.Core.Data;
using T802.Core.Domain.Logging;
using T802.Core.Domain.Students;
using T802.Data;

namespace T802.Services.Logging
{
    /// <summary>
    /// Student activity service
    /// </summary>
    public class StudentActivityService : IStudentActivityService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ACTIVITYTYPE_ALL_KEY = "T802.activitytype.all";

        #endregion

        #region Fields

        /// <summary>
        /// Cache manager
        /// </summary>
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IWorkContext _workContext;
        #endregion
        
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="activityLogRepository">Activity log repository</param>
        /// <param name="activityLogTypeRepository">Activity log type repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="dbContext">DB context</param>>
        /// <param name="dataProvider">WeData provider</param>
        /// <param name="commonSettings">Common settings</param>
        public StudentActivityService(ICacheManager cacheManager,
            IRepository<ActivityLog> activityLogRepository,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IWorkContext workContext)
        {
            this._cacheManager = cacheManager;
            this._activityLogRepository = activityLogRepository;
            this._activityLogTypeRepository = activityLogTypeRepository;
            this._workContext = workContext;
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class ActivityLogTypeForCaching
        {
            public int Id { get; set; }
            public string SystemKeyword { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
        }

        #endregion

        #region Utitlies

        /// <summary>
        /// Gets all activity log types (class for caching)
        /// </summary>
        /// <returns>Activity log types</returns>
        protected virtual IList<ActivityLogTypeForCaching> GetAllActivityTypesCached()
        {
            //cache
            string key = string.Format(ACTIVITYTYPE_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var result = new List<ActivityLogTypeForCaching>();
                var activityLogTypes = GetAllActivityTypes();
                foreach (var alt in activityLogTypes)
                {
                    var altForCaching = new ActivityLogTypeForCaching
                    {
                        Id = alt.Id,
                        SystemKeyword = alt.SystemKeyword,
                        Name = alt.Name,
                        Enabled = alt.Enabled
                    };
                    result.Add(altForCaching);
                }
                return result;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        public virtual IList<ActivityLogType> GetAllActivityTypes()
        {
            var query = from alt in _activityLogTypeRepository.Table
                orderby alt.Name
                select alt;
            var activityLogTypes = query.ToList();
            return activityLogTypes;
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        public virtual ActivityLog InsertActivity(string systemKeyword,
            string comment, params object[] commentParams)
        {
            return InsertActivity(systemKeyword, comment, _workContext.CurrentStudent, commentParams);
        }
        

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="student">The student</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        public virtual ActivityLog InsertActivity(string systemKeyword, 
            string comment, Student student, params object[] commentParams)
        {
            if (student == null)
                return null;

            var activityTypes = GetAllActivityTypesCached();
            var activityType = activityTypes.ToList().Find(at => at.SystemKeyword == systemKeyword);
            if (activityType == null || !activityType.Enabled)
                return null;

            comment = CommonHelper.EnsureNotNull(comment);
            comment = string.Format(comment, commentParams);
            comment = CommonHelper.EnsureMaximumLength(comment, 4000);

            

            var activity = new ActivityLog();
            activity.ActivityLogTypeId = activityType.Id;
            activity.Student = student;
            activity.Comment = comment;
            activity.CreatedOnUtc = DateTime.UtcNow;

            _activityLogRepository.Insert(activity);

            return activity;
        }
        #endregion

    }
}
