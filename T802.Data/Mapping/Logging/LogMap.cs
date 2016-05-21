using T802.Core.Domain.Logging;

namespace T802.Data.Mapping.Logging
{
    public partial class LogMap : T802EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.ToTable("Log");
            this.HasKey(l => l.Id);
            this.Property(l => l.ShortMessage).IsRequired();
            this.Property(l => l.IpAddress).HasMaxLength(200);

            this.Ignore(l => l.LogLevel);

            this.HasOptional(l => l.Student)
                .WithMany()
                .HasForeignKey(l => l.StudentId)
            .WillCascadeOnDelete(true);

        }
    }
}