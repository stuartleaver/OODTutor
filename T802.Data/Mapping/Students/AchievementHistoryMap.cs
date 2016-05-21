using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public class AchievementHistoryMap : T802EntityTypeConfiguration<AchievementHistory>
    {
        public AchievementHistoryMap()
        {
            this.ToTable("AchievementHistory");
            this.HasKey(ah => ah.Id);

            this.HasRequired(ah => ah.Student)
                .WithMany(c => c.AchievementHistory)
                .HasForeignKey(ah => ah.StudentId);

            this.HasRequired(ah => ah.Achievement)
                .WithMany(c => c.AchievementHistory)
                .HasForeignKey(ah => ah.AchievementId);
        }
    }
}
