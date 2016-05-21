using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;

namespace T802.Web.Models.Quiz
{
    [Validator(typeof(QuizCreateModel))]
    public partial class QuizCreateModel : BaseT802Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<QuizQuestionModel> Questions { get; set; } 
    }
}