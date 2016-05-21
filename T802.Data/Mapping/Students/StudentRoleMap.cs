using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public partial class StudentRoleMap : T802EntityTypeConfiguration<StudentRole>
    {
        public StudentRoleMap()
        {
            this.ToTable("StudentRole");
            this.HasKey(cr => cr.Id);
            this.Property(cr => cr.Name).IsRequired().HasMaxLength(255);
            this.Property(cr => cr.SystemName).HasMaxLength(255);
        }
    }
}