using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using T802.Core;
using T802.Core.Domain.Students;
using T802.Data.Mapping;

namespace T802.Data
{
    public class T802Context : DbContext, IDbContext
    {
        public T802Context() : base("T802Context")
        {
        }
        
        //public DbSet<Student> Students { get; set; }
        //public DbSet<StudentRole> StudentRoles { get; set; }
        //public DbSet<ActivityLogMap> ActivityLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(T802EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            
            base.OnModelCreating(modelBuilder);
        }


        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public System.Collections.Generic.IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
