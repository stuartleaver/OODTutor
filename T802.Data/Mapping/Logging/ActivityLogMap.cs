using T802.Core.Domain.Logging;

namespace T802.Data.Mapping.Logging
{
    public partial class ActivityLogMap : T802EntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.ToTable("ActivityLog");
            this.HasKey(al => al.Id);
            this.Property(al => al.Comment).IsRequired();

            this.HasRequired(al => al.ActivityLogType)
                .WithMany()
                .HasForeignKey(al => al.ActivityLogTypeId);

            this.HasRequired(al => al.Student)
                .WithMany()
                .HasForeignKey(al => al.StudentId);
        }
    }
}
