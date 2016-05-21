using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public partial class StudentMap : T802EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            this.ToTable("Student");
            this.HasKey(c => c.Id);
            this.Property(u => u.Username).HasMaxLength(1000);

            this.HasMany(c => c.StudentRoles)
                .WithMany()
                .Map(m => m.ToTable("Student_StudentRole_Mapping"));
        }
    }
}