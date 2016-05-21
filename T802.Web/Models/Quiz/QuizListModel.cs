using System;
using System.Collections.Generic;

namespace T802.Web.Models.Quiz
{
    public class QuizListModel : BaseT802Model
    {
        public IList<QuizModel> QuizList { get; set; } 
    }
}