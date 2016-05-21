using System.Collections.Generic;

namespace T802.Services.Students 
{
    public class StudentRegistrationResult 
    {
        public IList<string> Errors { get; set; }

        public StudentRegistrationResult() 
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
