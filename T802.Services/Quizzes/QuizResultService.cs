using System.Linq;
using T802.Core.Data;
using T802.Core.Domain.Quizzes;

namespace T802.Services.Quizzes
{
    public class QuizResultService : IQuizResultService
    {
        private readonly IRepository<QuizResult> _quizResultRepository;

        public QuizResultService(IRepository<QuizResult> quizResultRepository)
        {
            this._quizResultRepository = quizResultRepository;
        }

        public float GetQuizResult(string username, int quizId)
        {
            var query = from r in _quizResultRepository.Table
                        where r.Student.Username == username && r.QuizId == quizId
                        select r;

            var quizResult = query.SingleOrDefault();
            return quizResult != null ? quizResult.Score : 0;
        }

        public QuizResult GetQuizResult(string id, string student)
        {
            var query = from r in _quizResultRepository.Table
                        orderby r.Id descending 
                        where r.Quiz.SystemName == id && r.Student.Username == student
                        select r;

            return query.FirstOrDefault();
        }
    }
}
