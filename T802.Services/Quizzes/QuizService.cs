using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Data;
using T802.Core.Domain.Quizzes;
using T802.Core.Domain.Students;

namespace T802.Services.Quizzes
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<Quiz> _quizRepository;
        private readonly IRepository<QuizQuestion> _quizQuestionRepository;
        private readonly IRepository<QuizAnswer> _quizAnswerRepository; 
        private readonly IRepository<QuizResult> _quizResultRepository;
        private readonly IRepository<QuizComment> _quizCommentRepository; 

        public QuizService(IRepository<Quiz> quizRepository,
            IRepository<QuizQuestion> quizQuestionRepository,
            IRepository<QuizAnswer> quizAnswerRepository,
            IRepository<QuizResult> quizResultRepository,
            IRepository<QuizComment> quizCommentRepository)
        {
            this._quizRepository = quizRepository;
            this._quizQuestionRepository = quizQuestionRepository;
            this._quizAnswerRepository = quizAnswerRepository;
            this._quizResultRepository = quizResultRepository;
            this._quizCommentRepository = quizCommentRepository;
        }

        public IList<Quiz>GetQuizList()
        {
            var query = from q in _quizRepository.Table
                orderby q.CreatedUtc descending
                where q.IsSystemQuiz == false && q.IsLevelQuiz == false
                select q;

            return query.ToList();
        }

        public Quiz GetQuizById(int quizId)
        {
            if (quizId == 0)
                return null;

            return _quizRepository.GetById(quizId);
        }

        public Quiz GetQuizBySystemName(string systemName)
        {
            var query = from q in _quizRepository.Table
                where q.SystemName == systemName
                select q;

            return query.ToList().SingleOrDefault();
        }

        public QuizQuestion GetQuizQuestionById(int questionId)
        {
            if (questionId == 0)
                return null;

            return _quizQuestionRepository.GetById(questionId);
        }

        public QuizAnswer GetQuizAnswerById(int id)
        {
            return _quizAnswerRepository.GetById(id);
        }

        public void GradeQuiz(QuizResult quizResult)
        {
            _quizResultRepository.Insert(quizResult);
        }

        public void UpdateQuizGrade(QuizResult quizResult)
        {
            _quizResultRepository.Update(quizResult);
        }

        public int CreateQuiz(Quiz quiz)
        {
            _quizRepository.Insert(quiz);
            return quiz.Id;
        }

        public int CreateQuestion(int quizId, string text, int points, List<QuizAnswer> answers)
        {
            var question = new QuizQuestion
            {
                QuizId = quizId,
                Question = text,
                Points = points,
                QuizAnswers = answers
            };

            _quizQuestionRepository.Insert(question);

            return question.Id;
        }

        public void EditQuestion(QuizQuestion question)
        {
            _quizQuestionRepository.Update(question);
        }

        public void CreateAnswer(int questionId, string text, bool isCorrect)
        {
            var answer = new QuizAnswer
            {
                QuestionId = questionId,
                Text = text,
                IsCorrect = isCorrect
            };
            _quizAnswerRepository.Insert(answer);
        }

        public void DeleteAnswerById(int id)
        {
            _quizAnswerRepository.Delete(_quizAnswerRepository.GetById(id));
        }

        public int GetNumberOfTimesQuizTaken(int id)
        {
            var query = from q in _quizResultRepository.Table
                where q.QuizId == id
                select q;

            return query.Count();
        }

        public IList<QuizComment> GetQuizCommentsByQuizId(int id)
        {
            var query = from c in _quizCommentRepository.Table
                where c.QuizId == id
                select c;

            return query.ToList();
        }

        public void CreateComment(int quizId, string comment)
        {
            var quizComment = new QuizComment
            {
                QuizId = quizId,
                CreatedUtc = DateTime.UtcNow,
                Comment = comment
            };
            _quizCommentRepository.Insert(quizComment);
        }

        public int GetNumberOfQuizComments(int id)
        {
            var query = from c in _quizCommentRepository.Table
                where c.QuizId == id
                select c;

            return query.Count();
        }

        public bool HasTakenQuiz(string systemName, string username)
        {
            var quiz = GetQuizBySystemName(systemName);

            var query = from r in _quizResultRepository.Table
                where r.QuizId == quiz.Id && r.Student.Username == username
                select r;

            return query.ToList().Count >= 1;
        }
    }
}
