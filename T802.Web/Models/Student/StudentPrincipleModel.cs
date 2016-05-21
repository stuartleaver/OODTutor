namespace T802.Web.Models.Student
{
    public class StudentPrincipleModel : BaseT802Model
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}