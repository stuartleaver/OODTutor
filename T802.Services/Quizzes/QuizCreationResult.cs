using System.Collections.Generic;

namespace T802.Services.Quizzes 
{
    public class QuizCreationResult 
    {
        public IList<string> Errors { get; set; }

        public QuizCreationResult() 
        {
            this.Errors = new List<string>();
        }

        public bool Success 
        {
            get { return this.Errors.Count == 0; }
        }

        public void AddError(string error) 
        {
            this.Errors.Add(error);
        }
    }
}
