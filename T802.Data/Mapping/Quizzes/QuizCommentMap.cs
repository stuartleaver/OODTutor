using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Domain.Quizzes;

namespace T802.Data.Mapping.Quizzes
{
    public partial class QuizCommentMap : T802EntityTypeConfiguration<QuizComment>
    {
        public QuizCommentMap()
        {
            this.ToTable("QuizComment");
            this.HasKey(q => q.Id);

            this.HasRequired(qr => qr.Quiz)
                .WithMany()
                .HasForeignKey(qr => qr.QuizId);
        }
    }
}
