using System.Data.Entity.ModelConfiguration;

namespace T802.Data.Mapping
{
    public abstract class T802EntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected T802EntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}