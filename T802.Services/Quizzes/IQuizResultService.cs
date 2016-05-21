using T802.Core.Domain.Quizzes;

namespace T802.Services.Quizzes
{
    public interface IQuizResultService
    {
        float GetQuizResult(string username, int id);
        QuizResult GetQuizResult(string id, string student);
    }
}
