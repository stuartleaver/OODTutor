using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T802.Web.Models.Quiz
{
    public class QuizCommentListModel : BaseT802Model
    {
        public IList<QuizCommentModel> Comments { get; set; }
        public int QuizId { get; set; }
        public int NewComment { get; set; }
    }
}