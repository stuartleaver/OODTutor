using System.Collections.Generic;
using T802.Core.Domain.Quizzes;

namespace T802.Services.Quizzes
{
    public partial interface IQuizService
    {
        IList<Quiz> GetQuizList();
        Quiz GetQuizById(int quizId);
        Quiz GetQuizBySystemName(string systemName);
        QuizQuestion GetQuizQuestionById(int questionId);
        QuizAnswer GetQuizAnswerById(int id);
        void GradeQuiz(QuizResult quiz);
        void UpdateQuizGrade(QuizResult quiz);
        int CreateQuiz(Quiz quiz);
        int CreateQuestion(int quizId, string question, int points, List<QuizAnswer> answers);
        void EditQuestion(QuizQuestion question);
        void CreateAnswer(int questionId, string answer, bool isCorrect);
        void DeleteAnswerById(int id);
        int GetNumberOfTimesQuizTaken(int id);
        IList<QuizComment> GetQuizCommentsByQuizId(int id);
        void CreateComment(int quizId, string comment);
        int GetNumberOfQuizComments(int id);
        bool HasTakenQuiz(string systemName, string username);
    }
}
