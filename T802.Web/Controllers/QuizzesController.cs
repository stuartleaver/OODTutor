using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using T802.Core;
using T802.Core.Domain.Quizzes;
using T802.Core.Domain.Students;
using T802.Services.Achievements;
using T802.Services.Logging;
using T802.Services.Quizzes;
using T802.Services.Students;
using T802.Web.Framework;
using T802.Web.Models.Quiz;

namespace T802.Web.Controllers
{
    [CustomAuthorizeAttribute("GameMethod", "TraditionalMethod")]
    public class QuizzesController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IWorkContext _workContext;
        private readonly IAchievementService _achievementService;
        private readonly IStudentActivityService _studentActivityService;
        private readonly IStudentService _studentService;
        private readonly IQuizResultService _quizResultService;

        public QuizzesController(IQuizService quizService,
            IWorkContext workContext,
            IAchievementService achievementService,
            IStudentActivityService studentActivityService,
            IStudentService studentService,
            IQuizResultService quizResultService)
        {
            this._quizService = quizService;
            this._workContext = workContext;
            this._achievementService = achievementService;
            this._studentActivityService = studentActivityService;
            this._studentService = studentService;
            this._quizResultService = quizResultService;
        }

        //
        // GET: /Quiz/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var quizzes = _quizService.GetQuizList();
            var model = new QuizListModel
            {
                QuizList = new List<QuizModel>()
            };

            foreach (var quiz in quizzes)
            {
                var quizModel = new QuizModel();
                PrepareQuizModel(quizModel, quiz);
                model.QuizList.Add(quizModel);
            }

            //activity log
            _studentActivityService.InsertActivity("Quiz.Student.List", "ActivityLog.Quiz.Student.List");

            return View(model);
        }

        public ActionResult Quiz(int id)
        {
            var quiz = _quizService.GetQuizById(id);

            if (_quizService.HasTakenQuiz(quiz.SystemName, User.Identity.Name))
                return RedirectToAction("Index", "Home");

            var model = new QuizModel();
            PrepareQuizModel(model, quiz);

            //activity log
            _studentActivityService.InsertActivity("Quiz.Take", "ActivityLog.Quiz.Take." + quiz.SystemName);

            return View(model);
        }

        [HttpPost]
        public ActionResult Grade(QuizModel quiz)
        {
            var numberOfCorrectAnswers = 0;
            var rewardPoints = 0;
            var answers = new List<QuizUserAnswer>();

            foreach (var question in quiz.Questions)
            {
                int correctAnswer = (from a in question.Answers
                                     where a.IsCorrect
                                     select a.Id).SingleOrDefault();

                if (question.SelectedAnswer == correctAnswer)
                {
                    numberOfCorrectAnswers += 1;
                    rewardPoints += question.Points;
                }

                answers.Add(new QuizUserAnswer
                {
                    QuestionId = question.Id,
                    AnswerId = question.SelectedAnswer
                });
            }

            var score = ((float)numberOfCorrectAnswers / (float)quiz.Questions.Count) * 100;
            quiz.StudentScore = (float)Math.Round(score, 3);

            var quizResult = new QuizResult
            {
                StudentId = _workContext.CurrentStudent.Id,
                AnsweredOnUtc = DateTime.UtcNow,
                Score = (float)score,
                QuizId = quiz.Id,
                Answers = answers
            };

            Achievement achievement = null;

            // If it is a level quiz, only store the grade if the pass mark has been met.
            if (quiz.IsLevelQuiz)
            {
                if (score >= quiz.PassMark)
                {
                    _quizService.GradeQuiz(quizResult);
                    achievement = _achievementService.GetAchievementBySystemName(quiz.AchivementSystemName);
                    quizResult.Student.AddLevelPointsHistoryEntry(rewardPoints, quiz.Id, "Passed quiz " + quiz.Id);

                    //activity log
                    _studentActivityService.InsertActivity("Quiz.Grade.Level.Passed",
                        "ActivityLog.Quiz.Grade.Level." + quiz.SystemName + ".Passed");
                }
                else
                    _studentActivityService.InsertActivity("Quiz.Grade.Level.Failed",
                        "ActivityLog.Quiz.Grade.Level." + quiz.SystemName + ".Failed");
            }
            else if (quiz.IsSystemQuiz)
            {
                _quizService.GradeQuiz(quizResult);
                achievement = _achievementService.GetAchievementBySystemName(quiz.AchivementSystemName);
                quizResult.Student.AddRewardPointsHistoryEntry(rewardPoints, "Completed quiz " + quiz.Id);

                //activity log
                _studentActivityService.InsertActivity("Quiz.Grade.System", "ActivityLog.Quiz.Grade.System." + quiz.SystemName);
            }
            else
            {
                _quizService.GradeQuiz(quizResult);
                achievement = _achievementService.GetAchievementBySystemName(quiz.AchivementSystemName);
                quizResult.Student.AddRewardPointsHistoryEntry(rewardPoints, "Completed quiz " + quiz.Id);

                //activity log
                _studentActivityService.InsertActivity("Quiz.Grade.Student", "ActivityLog.Quiz.Grade.Student." + quiz.SystemName);
            }

            _quizService.UpdateQuizGrade(quizResult);

            if (achievement != null)
            {
                quizResult.Student.AddAchievementHistoryEntry(achievement);
                _studentService.UpdateStudent(quizResult.Student);
            }

            //if (quiz.IsSystemQuiz || (quiz.IsLevelQuiz && score < quiz.PassMark))
            //    return RedirectToAction("Index", "Home");

            return View(quiz);
        }

        public ActionResult Create()
        {
            var model = new QuizModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(QuizModel model)
        {
            int quizId = 0;

            if (_quizService.GetQuizBySystemName(model.SystemName) != null)
                ModelState.AddModelError("", "Quiz name already exists");

            if (!ModelState.IsValid)
                return View(model);

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var quiz = new Quiz
            {
                Name = model.Name,
                Description = model.Description,
                SystemName = textInfo.ToTitleCase(model.Name).Replace(" ", ""),
                CreatedUtc = DateTime.UtcNow,
                CreatedBy = _studentService.GetStudentByUsername(User.Identity.Name),
                IsSystemQuiz = false,
                IsStudentQuiz = true,
                AchivementSystemName = "CompletedStudentCreatedQuiz"
            };
            _quizService.CreateQuiz(quiz);

            quizId = quiz.Id;

            // Award achievement
            var student = _studentService.GetStudentByUsername(User.Identity.Name);
            student.AddAchievementHistoryEntry(_achievementService.GetAchievementBySystemName(SystemStudentAchievementNames.CreatedStudentCreatedQuiz));
            _achievementService.GetAchievementBySystemName(SystemStudentAchievementNames.CompletedStudentCreatedQuiz);

            //activity log
            _studentActivityService.InsertActivity("Quiz.Create", "ActivityLog.Quiz.Create");

            return RedirectToAction("Edit", new { id = quizId });
        }

        public ActionResult Edit(int id)
        {
            var quiz = _quizService.GetQuizById(id);

            var model = new QuizModel();
            PrepareQuizModel(model, quiz);

            return View(model);
        }

        public ActionResult CreateQuestion(int id)
        {
            var model = new QuizQuestionModel
            {
                QuizId = id,
                Answers = new List<QuizAnswerModel>()
            };

            for (var i = 0; i <= 3; i++)
                model.Answers.Add(new QuizAnswerModel());

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(QuizQuestionModel model)
        {
            var numberOfAnswers = model.Answers.Where(a => !String.IsNullOrWhiteSpace(a.Text)).Count();

            if (numberOfAnswers < 2)
                ModelState.AddModelError("", "Question requires two or more answers");

            if (model.SelectedAnswer + 1 > numberOfAnswers)
                ModelState.AddModelError("", "The selected answer does not have a corresponding answer");

            if (!ModelState.IsValid)
                return View(model);

            // Set the correct answer
            model.Answers[model.SelectedAnswer].IsCorrect = true;
            model.SelectedAnswer = 0;

            var answers = new List<QuizAnswer>();
            foreach (var answer in model.Answers)
            {
                if (!String.IsNullOrWhiteSpace(answer.Text))
                {
                    var item = new QuizAnswer
                    {
                        IsCorrect = answer.IsCorrect,
                        Text = answer.Text
                    };
                    answers.Add(item);
                }
            }

            var questionId = _quizService.CreateQuestion(model.QuizId, model.Question, model.Points, answers);

            for (int i = 0; i < answers.Count; i++)
            {
                //activity log
                _studentActivityService.InsertActivity("Quiz.Create.Question.Answer", "ActivityLog.Quiz.Create.Question.Answer");
            }

            //activity log
            _studentActivityService.InsertActivity("Quiz.Create.Question", "ActivityLog.Quiz.Create.Question");

            return RedirectToAction("Edit", new { id = model.QuizId });
        }

        public ActionResult DeleteAnswer(int id)
        {
            var answer = _quizService.GetQuizAnswerById(id);
            _quizService.DeleteAnswerById(id);
            return RedirectToAction("EditQuestion", new { id = answer.QuestionId });
        }

        public ActionResult Comment(int id)
        {
            var comments = _quizService.GetQuizCommentsByQuizId(id);
            var model = new QuizCommentListModel
            {
                Comments = new List<QuizCommentModel>(),
                QuizId = id
            };

            foreach (var comment in comments)
            {
                var quizComment = new QuizCommentModel();
                PrepareQuizCommentModel(quizComment, comment);
                model.Comments.Add(quizComment);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateComment(QuizCommentModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _quizService.CreateComment(model.QuizId, model.Comment);
            return RedirectToAction("List");
        }

        public ActionResult Result(string id, string student)
        {
            var model = new QuizResultModel();
            var quizResult = _quizResultService.GetQuizResult(id, student);

            PrepareQuizResultModel(model, quizResult);

            //activity log
            _studentActivityService.InsertActivity("Quiz.Results", "ActivityLog.Quiz.Results." + model.SystemName);

            return View(model);
        }

        [NonAction]
        private void PrepareQuizModel(QuizModel model, Quiz quiz)
        {
            if (quiz == null)
                throw new ArgumentNullException("quiz");

            if (model == null)
                throw new ArgumentNullException("model");

            model.Id = quiz.Id;
            model.QuizGuid = quiz.QuizGuid;
            model.IsSystemQuiz = quiz.IsSystemQuiz;
            model.SystemName = quiz.SystemName;
            model.IsLevelQuiz = quiz.IsLevelQuiz;
            model.IsStudentQuiz = quiz.IsStudentQuiz;
            model.PassMark = quiz.PassMark;
            model.AchivementSystemName = quiz.AchivementSystemName;
            model.Name = quiz.Name;
            model.Description = quiz.Description;
            model.Questions = new List<QuizQuestionModel>();
            model.NumberOfTimesQuizTaken = _quizService.GetNumberOfTimesQuizTaken(quiz.Id);
            model.NumberOfQuizComments = _quizService.GetNumberOfQuizComments(quiz.Id);
            model.CreatedBy = quiz.CreatedBy.Username;
            model.HasTakenQuiz = _quizService.HasTakenQuiz(quiz.SystemName, User.Identity.Name);

            if (model.HasTakenQuiz)
                model.StudentScore = _quizResultService.GetQuizResult(User.Identity.Name, quiz.Id);

            // Questions
            foreach (var question in quiz.QuizQuestions)
            {
                // Answers
                var answers = new List<QuizAnswerModel>();
                foreach (var answer in question.QuizAnswers)
                {
                    var answerModel = new QuizAnswerModel
                    {
                        Id = answer.Id,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect
                    };
                    answers.Add(answerModel);
                }

                var questionModel = new QuizQuestionModel
                {
                    Id = question.Id,
                    Question = question.Question,
                    Answers = answers,
                    Points = question.Points,
                    Image = question.Image,
                    Hint = question.Hint
                };
                model.Questions.Add(questionModel);
            }
        }

        [NonAction]
        private void PrepareQuizCommentModel(QuizCommentModel model, QuizComment comment)
        {
            model.CommentId = comment.CommentId;
            model.CreatedUtc = comment.CreatedUtc;
            model.Comment = comment.Comment;
            model.QuizId = comment.QuizId;
        }

        [NonAction]
        private void PrepareQuizResultModel(QuizResultModel model, QuizResult quizResult)
        {
            if (quizResult == null)
                throw new ArgumentNullException("quizResult");

            if (model == null)
                throw new ArgumentNullException("model");

            model.QuizId = quizResult.QuizId;
            model.SystemName = quizResult.Quiz.SystemName;
            model.StudentScore = quizResult.Score;
            model.PassMark = quizResult.Quiz.PassMark;
            model.IsSystemQuiz = quizResult.Quiz.IsSystemQuiz;
            model.IsLevelQuiz = quizResult.Quiz.IsLevelQuiz;
            model.Questions = new List<QuizQuestionModel>();

            foreach (var question in quizResult.Quiz.QuizQuestions.OrderBy(q => q.Id))
            {
                var quizQuestion = new QuizQuestionModel();

                quizQuestion.Question = question.Question;
                quizQuestion.Image = question.Image;
                quizQuestion.Hint = question.Hint;

                quizQuestion.Answers = new List<QuizAnswerModel>();
                foreach (var answer in question.QuizAnswers)
                {
                    var quizAnswer = new QuizAnswerModel();

                    quizAnswer.Id = answer.Id;
                    quizAnswer.QuestionId = answer.QuestionId;
                    quizAnswer.Text = answer.Text;
                    quizAnswer.IsCorrect = answer.IsCorrect;

                    quizQuestion.Answers.Add(quizAnswer);
                }
                quizQuestion.SelectedAnswer =
                    quizResult.Answers.Where(a => a.QuestionId == question.Id).Select(q => q.AnswerId).FirstOrDefault();

                model.Questions.Add(quizQuestion);
            }
        }
    }
}