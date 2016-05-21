using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using T802.Core.Data;
using T802.Core.Domain.Logging;
using T802.Core.Domain.Quizzes;
using T802.Core.Domain.Students;
using T802.Services.Students;

namespace T802.Services.Installation
{
    public class InstallationService : IInstallationService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<StudentRole> _studentRoleRepository;
        private readonly IStudentRegistrationService _studentRegistrationService;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<Achievement> _achievementRepository;
        private readonly IRepository<AchievementLevel> _achievementLevelRepository;
        private readonly IRepository<Quiz> _quizRepository;

        private Student administrativeUser;

        public InstallationService(IRepository<Student> studentRepository,
            IRepository<StudentRole> studentRoleRepository,
            IStudentRegistrationService studentRegistrationService,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IRepository<Achievement> achievementRepository,
            IRepository<AchievementLevel> achievementLevelRepository,
            IRepository<Quiz> quizRepository)
        {
            this._studentRepository = studentRepository;
            this._studentRoleRepository = studentRoleRepository;
            this._studentRegistrationService = studentRegistrationService;
            this._activityLogTypeRepository = activityLogTypeRepository;
            this._achievementRepository = achievementRepository;
            this._achievementLevelRepository = achievementLevelRepository;
            this._quizRepository = quizRepository;
        }

        public void InstallData(string username, string password, bool installSampleData = true)
        {
            InstallStudentsAndUsers(username, password);
            InstallActivityLogTypes();
            InstallAchievementLevels();
            InstallAchievements();
            InstallQuizzes();
        }

        public bool IsDatabaseInstalled()
        {
            var query = from s in _studentRepository.Table
                where s.IsSystemAccount == true
                select s;

            return query.Any();
        }

        private void InstallStudentsAndUsers(string username, string password)
        {
            var crAdministrators = new StudentRole
            {
                Name = "Administrators",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemStudentRoleNames.Administrators,
            };
            var crGameMethod = new StudentRole
            {
                Name = "Game Method",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemStudentRoleNames.Game,
            };
            var crTraditionalMethod = new StudentRole
            {
                Name = "Traditional Method",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemStudentRoleNames.Traditional,
            };
            var studentRoles = new List<StudentRole>
            {
                crAdministrators,
                crGameMethod,
                crTraditionalMethod,
            };
            studentRoles.ForEach(cr => _studentRoleRepository.Insert(cr));

            //admin user
            var adminUser = new Student
            {
                StudentGuid = Guid.NewGuid(),
                Username = username,
                Password = "",
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                IsSystemAccount = true
            };
            adminUser.StudentRoles.Add(crAdministrators);
            _studentRepository.Insert(adminUser);

            // Set password of admin account
            _studentRegistrationService.ChangePassword(new ChangePasswordRequest(username, false, password));

            administrativeUser = adminUser;
        }

        protected virtual void InstallActivityLogTypes()
        {
            var activityLogTypes = new List<ActivityLogType>
                                      {
                                          //admin area activities
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewStudent",
                                                  Enabled = true,
                                                  Name = "Add a New Student"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewStudentRole",
                                                  Enabled = true,
                                                  Name = "Add a New Student Role"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditStudent",
                                                  Enabled = true,
                                                  Name = "Edit a Student"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditStudentRole",
                                                  Enabled = true,
                                                  Name = "Edit a Student Role"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Login",
                                                  Enabled = true,
                                                  Name = "Game Login"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Registration",
                                                  Enabled = true,
                                                  Name = "Game Registration"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Logout",
                                                  Enabled = true,
                                                  Name = "Game Logout"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Create",
                                                  Enabled = true,
                                                  Name = "Quiz Create"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Take",
                                                  Enabled = true,
                                                  Name = "Quiz Take"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Grade.System",
                                                  Enabled = true,
                                                  Name = "System Quiz Completed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Grade.Student",
                                                  Enabled = true,
                                                  Name = "Student Quiz Completed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Grade.Level.Passed",
                                                  Enabled = true,
                                                  Name = "Level Quiz Passed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Grade.Level.Failed",
                                                  Enabled = true,
                                                  Name = "Level Quiz Failed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Student.List",
                                                  Enabled = true,
                                                  Name = "Viewed Student Quiz List"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Create.Question",
                                                  Enabled = true,
                                                  Name = "Created Student Quiz Question"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Create.Question.Answer",
                                                  Enabled = true,
                                                  Name = "Created Student Quiz Question Answer"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Quiz.Results",
                                                  Enabled = true,
                                                  Name = "Viewed Quiz Results"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Home.Page.Game",
                                                  Enabled = true,
                                                  Name = "Game Home Page"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Home.Page.Traditional",
                                                  Enabled = true,
                                                  Name = "Traditional Home Page"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.SingleResponsibilityPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Viewed - Single Responsibility Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.OpenClosedPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Viewed - Open Closed Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.LiskovSubstitutionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Viewed - Liskov Substitution Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.InterfaceSegregationPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Viewed - Interface Segregation Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.DependencyInversionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Viewed - Dependency Inversion Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.PDF.SingleResponsibilityPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material PDF Viewed - Single Responsibility Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.PDF.OpenClosedPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material PDF Viewed - Open Closed Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.PDF.LiskovSubstitutionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material PDF Viewed - Liskov Substitution Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.PDF.InterfaceSegregationPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material PDF Viewed - Interface Segregation Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.PDF.DependencyInversionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material PDF Viewed - Dependency Inversion Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.Video.SingleResponsibilityPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Video Viewed - Single Responsibility Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.Video.OpenClosedPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Video Viewed - Open Closed Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.Video.LiskovSubstitutionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Video Viewed - Liskov Substitution Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.Video.InterfaceSegregationPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Video Viewed - Interface Segregation Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.Video.DependencyInversionPrinciple",
                                                  Enabled = true,
                                                  Name = "Course Material Video Viewed - Dependency Inversion Principle"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Leaderboard",
                                                  Enabled = true,
                                                  Name = "Leaderboard Viewed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Leaderboard.Achievements",
                                                  Enabled = true,
                                                  Name = "Achievements Leaderboard Viewed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Achievements.List",
                                                  Enabled = true,
                                                  Name = "Achievements List Viewed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Course.Material.SampleCode",
                                                  Enabled = true,
                                                  Name = "Downloaded the Sample Code"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.PasswordRecovery",
                                                  Enabled = true,
                                                  Name = "Password Recovery Page"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.PasswordRecovery.Success",
                                                  Enabled = true,
                                                  Name = "Password Recovery was a Success"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.PasswordRecovery.Fail",
                                                  Enabled = true,
                                                  Name = "Password Recovery Failed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Contact.Form",
                                                  Enabled = true,
                                                  Name = "Password Recovery Failed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Contact.Form.Sent",
                                                  Enabled = true,
                                                  Name = "Password Recovery Failed"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "Game.Survey",
                                                  Enabled = true,
                                                  Name = "Game Survey"
                                              }
                                      };
            activityLogTypes.ForEach(alt => _activityLogTypeRepository.Insert(alt));
        }

        private void InstallAchievementLevels()
        {
            var achievementLevels = new List<AchievementLevel>
            {
                new AchievementLevel
                {
                    AchievementLevelId = SystemAchievementLevels.Bronze,
                    AchievementLevelDescription = "Bronze"
                },
                new AchievementLevel
                {
                    AchievementLevelId = SystemAchievementLevels.Silver,
                    AchievementLevelDescription = "Silver"
                },
                new AchievementLevel
                {
                    AchievementLevelId = SystemAchievementLevels.Gold,
                    AchievementLevelDescription = "Gold"
                },
                new AchievementLevel
                {
                    AchievementLevelId = SystemAchievementLevels.Platinum,
                    AchievementLevelDescription = "Platinum"
                }
            };

            achievementLevels.ForEach(al => _achievementLevelRepository.Insert(al));
        }

        private void InstallAchievements()
        {
            var achievementTypes = new List<Achievement>
            {
                new Achievement
                {
                    SystemName = "Registered",
                    Active = true,
                    Name = "Registered",
                    Description = "Registered on the site",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "CompletedInitialQuiz",
                    Active = true,
                    Name = "Completed Initial Quiz",
                    Description = "Completed the initial quiz at the start of the game",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "CompletedFinalQuiz",
                    Active = true,
                    Name = "Completed Final Quiz",
                    Description = "Completed final quiz at the end of the game",
                    AchievementLevelId = SystemAchievementLevels.Gold
                },
                new Achievement
                {
                    SystemName = "CreatedStudentCreatedQuiz",
                    Active = true,
                    Name = "Created a Student Created Quiz",
                    Description = "Created a quiz that can be taken by fellow students",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "CompletedStudentCreatedQuiz",
                    Active = true,
                    Name = "Completed Student Created Quiz",
                    Description = "Completed a quiz created by a fellow student",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "EarntAllAchievements",
                    Active = true,
                    Name = "Earnt all available achievements",
                    Description = "Earnt all achievements that are avilable in the game",
                    AchievementLevelId = SystemAchievementLevels.Platinum
                },
                new Achievement
                {
                    SystemName = "SRPBronzeLevel",
                    Active = true,
                    Name = "SRP Bronze Level",
                    Description = "Passed the Single Responsibility Principle bronze quiz",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "SRPSilverLevel",
                    Active = true,
                    Name = "SRP Silver Level",
                    Description = "Passed the Single Responsibility Principle silver quiz",
                    AchievementLevelId = SystemAchievementLevels.Silver
                },
                new Achievement
                {
                    SystemName = "SRPGoldLevel",
                    Active = true,
                    Name = "SRP Gold Level",
                    Description = "Passed the Single Responsibility Principle gold quiz",
                    AchievementLevelId = SystemAchievementLevels.Gold
                },
                new Achievement
                {
                    SystemName = "OCPBronzeLevel",
                    Active = true,
                    Name = "OCP Bronze Level",
                    Description = "Passed the Open/Closed Principle bronze quiz",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "OCPSilverLevel",
                    Active = true,
                    Name = "OCP Silver Level",
                    Description = "Passed the Open/Closed Principle silver quiz",
                    AchievementLevelId = SystemAchievementLevels.Silver
                },
                new Achievement
                {
                    SystemName = "OCPGoldLevel",
                    Active = true,
                    Name = "OCP Gold Level",
                    Description = "Passed the Open/Closed Principle gold quiz",
                    AchievementLevelId = SystemAchievementLevels.Gold
                },
                new Achievement
                {
                    SystemName = "LSPBronzeLevel",
                    Active = true,
                    Name = "LSP Bronze Level",
                    Description = "Passed the Liskov Substitution Principle bronze quiz",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "LSPSilverLevel",
                    Active = true,
                    Name = "LSP Silver Level",
                    Description = "Passed the Liskov Substitution Principle silver quiz",
                    AchievementLevelId = SystemAchievementLevels.Silver
                },
                new Achievement
                {
                    SystemName = "LSPGoldLevel",
                    Active = true,
                    Name = "LSP Gold Level",
                    Description = "Passed the Liskov Substitution Principle gold quiz",
                    AchievementLevelId = SystemAchievementLevels.Gold
                },
                new Achievement
                {
                    SystemName = "ISPBronzeLevel",
                    Active = true,
                    Name = "ISP Bronze Level",
                    Description = "Passed the Interface Segregation Principle bronze quiz",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "ISPSilverLevel",
                    Active = true,
                    Name = "ISP Silver Level",
                    Description = "Passed the Interface Segregation Principle silver quiz",
                    AchievementLevelId = SystemAchievementLevels.Silver
                },
                new Achievement
                {
                    SystemName = "ISPGoldLevel",
                    Active = true,
                    Name = "ISP Gold Level",
                    Description = "Passed the Interface Segregation Principle gold quiz",
                    AchievementLevelId = SystemAchievementLevels.Gold
                },
                new Achievement
                {
                    SystemName = "DIPBronzeLevel",
                    Active = true,
                    Name = "DIP Bronze Level",
                    Description = "Passed the Dependency Inversion Principle bronze quiz",
                    AchievementLevelId = SystemAchievementLevels.Bronze
                },
                new Achievement
                {
                    SystemName = "DIPSilverLevel",
                    Active = true,
                    Name = "DIP Silver Level",
                    Description = "Passed the Dependency Inversion Principle silver quiz",
                    AchievementLevelId = SystemAchievementLevels.Silver
                },
                new Achievement
                {
                    SystemName = "DIPGoldLevel",
                    Active = true,
                    Name = "DIP Gold Level",
                    Description = "Passed the Dependency Inversion Principle gold quiz",
                    AchievementLevelId = SystemAchievementLevels.Gold
                }
            };

            achievementTypes.ForEach(ach => _achievementRepository.Insert(ach));
        }

        private void InstallQuizzes()
        {
            InstallInitialQuiz();
            InstallFinalQuiz();
            InstallSRPQuizzes();
            InstallOCPQuizzes();
            InstallLSPQuizzes();
            InstallISPQuizzes();
            InstallDIPQuizzes();
        }

        private void InstallInitialQuiz()
        {
            #region Question 1

            var answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Open Closed Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                }
            };

            var questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Which of the following principles states 'A class should have only one reason to change'?",
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following diagram a good example of the Single Responsibility Principle?",
                Image = "Quiz/RectangleClass.png",
                Hint = "No, because the Rectangle class has two responsibilities.",
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The state or fact of having a duty to deal with something",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A reason for change",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Having control over someone.",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "In the Single Responsibility Principle, what is a 'responsibility' defined as?",
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Cohesion",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "High Coupling",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "OCL",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Low Coupling",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following other Object-Oriented design principles does the Single Responsibility Principle help with?",
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "True",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "False",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following statement correct about the Open-Closed Principle? 'Software entities should be closed for extension, but open for modification'",
                QuizAnswers = answers
            });

            #endregion Question 5

            #region Question 6

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Open-Closed Principle?",
                Image = "Quiz/FindUsers.png",
                Hint = "More UserSearchTypes could be added",
                QuizAnswers = answers
            });

            #endregion Question 6

            #region Question 7

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Anytime you change code, you have the potential to break it.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Sometimes you can't change libraries",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Maybe have to change code in many different places to add support for a certain type of situation",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "In the Open-Closed Principle, which of the following matter?",
                QuizAnswers = answers
            });

            #endregion Question 7

            #region Question 8

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above.",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which following principle states 'Derived classes must be substitutable for their base classes.'",
                QuizAnswers = answers
            });

            #endregion Question 8

            #region Question 9

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following class diagram in violation of the Liskov Substitution Principle?",
                Image = "Quiz/BirdClass.png",
                Hint = "Ostrich is a Bird (definitely it is!) and hence it inherits the Bird class. Now, can it fly? No!",
                QuizAnswers = answers
            });

            #endregion Question 9

            #region Question 10

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Interface Segregation Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Open-Closed Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What principle does the before and after show in the following diagram?",
                Image = "Quiz/WhichPrincipleIsThis.png",
                QuizAnswers = answers
            });

            #endregion Question 10

            #region Question 11

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Thin interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A medium interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A fat interface",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "If you implement an interface and have to throw an exception in a method because you don't support it, is the interface said to be a:",
                Hint = "A interface with to many methods is said to be a 'Fat interface'",
                QuizAnswers = answers
            });

            #endregion Question 11

            #region Question 12

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Abstractions",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Low level modules",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The details",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What should high level modules depend upon?",
                QuizAnswers = answers
            });

            #endregion Question 12

            #region Question 13

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Interface Segregation Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Liskov's Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Dependency Inversion Principle",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Tightly coupled classes are a sign of a violation of which SOLID Principle?",
                QuizAnswers = answers
            });

            #endregion Question 13

            #region Question 14

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Sometimes",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Should your domain model know how data access is done?",
                QuizAnswers = answers
            });

            #endregion Question 14

            #region Question 15

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Dependency Inversion Principle?",
                Image = "Quiz/GetProductService.png",
                QuizAnswers = answers
            });

            #endregion Question 15

            #region Question 16

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Interface Segregation Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "The Open Closed Principle",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which SOLID Principle can an IoC container help with?",
                QuizAnswers = answers
            });

            #endregion Question 16

            #region Question 17

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Mocks",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Stubs",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Fakes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following can be used without implementing the Dependency Inversion Principle",
                QuizAnswers = answers
            });

            #endregion Question 17

            #region Question 18

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is a subclass that modifies, rather than extends, the external observable behavior of it's parent class a violation of SOLID design principles?",
                QuizAnswers = answers
            });

            #endregion Question 18

            var quizzes = new List<Quiz>
            {
                new Quiz
                {
                    SystemName = "InitialQuiz",
                    Description = "This is the first quiz that will be used to benchmark your knowledge of SOLID. Once you have completed the final quiz, a comparision will be made as to the level of improvement between the two results.",
                    Name = "Initial Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = true,
                    IsLevelQuiz = false,
                    IsStudentQuiz = false,
                    AchivementSystemName = "CompletedInitialQuiz",
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            };

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallFinalQuiz()
        {
            #region Question 1

            var answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Open Closed Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                }
            };

            var questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Which of the following principles states 'A class should have only one reason to change'?",
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following diagram a good example of the Single Responsibility Principle?",
                Image = "Quiz/RectangleClass.png",
                Hint = "No, because the Rectangle class has two responsibilities.",
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The state or fact of having a duty to deal with something",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A reason for change",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Having control over someone.",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "In the Single Responsibility Principle, what is a 'responsibility' defined as?",
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Cohesion",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "High Coupling",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "OCL",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Low Coupling",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following other Object-Oriented design principles does the Single Responsibility Principle help with?",
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "True",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "False",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following statement correct about the Open-Closed Principle? 'Software entities should be closed for extension, but open for modification'",
                QuizAnswers = answers
            });

            #endregion Question 5

            #region Question 6

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Open-Closed Principle?",
                Image = "Quiz/FindUsers.png",
                Hint = "More UserSearchTypes could be added",
                QuizAnswers = answers
            });

            #endregion Question 6

            #region Question 7

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Anytime you change code, you have the potential to break it.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Sometimes you can't change libraries",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Maybe have to change code in many different places to add support for a certain type of situation",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "In the Open-Closed Principle, which of the following matter?",
                QuizAnswers = answers
            });

            #endregion Question 7

            #region Question 8

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above.",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which following principle states 'Derived classes must be substitutable for their base classes.'",
                QuizAnswers = answers
            });

            #endregion Question 8

            #region Question 9

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following class diagram in violation of the Liskov Substitution Principle?",
                Image = "Quiz/BirdClass.png",
                Hint = "Ostrich is a Bird (definitely it is!) and hence it inherits the Bird class. Now, can it fly? No!",
                QuizAnswers = answers
            });

            #endregion Question 9

            #region Question 10

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Interface Segregation Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Open-Closed Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What principle does the before and after show in the following diagram?",
                Image = "Quiz/WhichPrincipleIsThis.png",
                QuizAnswers = answers
            });

            #endregion Question 10

            #region Question 11

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Thin interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A medium interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A fat interface",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "If you implement an interface and have to throw an exception in a method because you don't support it, is the interface said to be a:",
                Hint = "A interface with to many methods is said to be a 'Fat interface'",
                QuizAnswers = answers
            });

            #endregion Question 11

            #region Question 12

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Abstractions",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Low level modules",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The details",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What should high level modules depend upon?",
                QuizAnswers = answers
            });

            #endregion Question 12

            #region Question 13

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Interface Segregation Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Liskov's Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Dependency Inversion Principle",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Tightly coupled classes are a sign of a violation of which SOLID Principle?",
                QuizAnswers = answers
            });

            #endregion Question 13

            #region Question 14

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Sometimes",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Should your domain model know how data access is done?",
                QuizAnswers = answers
            });

            #endregion Question 14

            #region Question 15

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Dependency Inversion Principle?",
                Image = "Quiz/GetProductService.png",
                QuizAnswers = answers
            });

            #endregion Question 15

            #region Question 16

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Interface Segregation Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "The Open Closed Principle",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which SOLID Principle can an IoC container help with?",
                QuizAnswers = answers
            });

            #endregion Question 16

            #region Question 17

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Mocks",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Stubs",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Fakes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following can be used without implementing the Dependency Inversion Principle",
                QuizAnswers = answers
            });

            #endregion Question 17

            #region Question 18

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is a subclass that modifies, rather than extends, the external observable behavior of it's parent class a violation of SOLID design principles?",
                QuizAnswers = answers
            });

            #endregion Question 18

            var quizzes = new List<Quiz>
            {
                new Quiz
                {
                    SystemName = "FinalQuiz",
                    Description = "The results of this quiz will be compared to your results from the first quiz to see what level of improvement there has been.",
                    Name = "Final Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = true,
                    IsLevelQuiz = false,
                    IsStudentQuiz = false,
                    AchivementSystemName = "CompletedFinalQuiz",
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            };

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallSRPQuizzes()
        {
            List<QuizAnswer> answers;
            List<QuizQuestion> questions;
            var quizzes = new List<Quiz>();

            #region Bronze

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "When your class does one thing and one thing only.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "When you class is responsible for only one thing.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "When your class only has one reason to change.",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "When you have converted your class to a singleton.",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "When do you know you have implemented the Single Responsibility Principle?",
                    Points = 10,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Objects should have a single responsibility.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "An object services should be focused on a single responsibility",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "You should only be able to create one instance of the object",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both A and B",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
                {
                    Question = "What does the Single Responsibility Principle ask for?",
                    Hint = "Every object in your system should have a single responsibility, and all the object's services should be focused on carrying out the single responsibility.",
                    Points = 10,
                    QuizAnswers = answers
                });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following class diagram a good example of the Single Responsibility Principle?",
                Image = "SRP/AutomobileClass.png",
                Hint =
                    "How many responsibilities does the class have? Could a 'Driver' class drive the Automobile for example?",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Can the single responsibility principle be broken if the class has to many dependencies?",
                
                Hint = "A constructor with too many input parameters implies many dependencies",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Improves maintainability",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Improves testing",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Improves quality",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following are benefits to using the single responsibilitie principle?",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.SRPBronzeLevel,
                    Description = "Bronze level quiz for the Single Responsibility Principle",
                    Name = "Single Responsibility Principle Bronze Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 40,
                    AchivementSystemName = SystemStudentAchievementNames.SRPBronzeLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Bronze

            #region Silver

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "3 including the automobile class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "4 including the automobile class",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "5 including the automobile class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "6 including the automobile class",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "How many classes could the class below be broken down into?",
                    Image = "SRP/AutomobileClass.png",
                    Hint = "A driver would drive the automobile, a mechanic would service it and a car wash would wash it.",
                    Points = 20,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Classes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Methods",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Packages",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the Above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which part of the system needs to have a single responsibility?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Can a method with to many parameters be a sign of a violation of the single responsibility principle?",
                Hint = "Think back to the similar question in the previous quiz. Think of method parameters as dependencies",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
               new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Does the following BankAccount class conform to the Single Responsibility Pattern?",
                Image = "SRP/BankAccount.png",
                Hint = "At the first sight, this class looks absolutely fine. However, it violates SRP.  The BankAccount class contains business rules and data management rules. And it'susually a good idea not to mix these responsibilities up. After a quick look at this class, we can see that these responsibilities might change for absolutelly different reasons, and binding them together would be a mistake.",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "If you use the word 'And' is a descriptive name for a class or method, are you possibly violating the single responsibility principle?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.SRPSilverLevel,
                    Description = "Silver level quiz for the Single Responsibility Principle",
                    Name = "Single Responsibility Principle Silver Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 60,
                    AchivementSystemName = SystemStudentAchievementNames.SRPSilverLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Silver

            #region Gold

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Singleton",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Cohesion",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Creator",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Abstraction",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "The Single Responsibility Principle is another name for which Object-Oriented Design principle?",
                    Points = 30,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Phone",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Game",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Neither",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following classes have multiple responsibilities?",
                Image = "SRP/SRPClasses.png",
                Hint = "The game class could be split into three classes, GameSession (login() & signup()) and PlayActions for the other three methods. The methods in the Phone class only handled by the phone.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "When applying the single responsibility principle, is a unit test with too many varients a bad sign?",
                Hint = "It might suggest that the class has too many responsibilities",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The constructor",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "ValidateEmail",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Greet",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the methods in the following TypeScript class  below violates the single responsibility principle?",
                Image = "SRP/TypeScriptClass.png",
                Hint = "It is the ValidateEmail method as that is not related with a person behaviour.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Maybe",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Does the following class conform to the single responsibility principle?",
                Image = "SRP/BookClass.png",
                Hint = "Now this may appear perfectly reasonable. We have no method dealing with persistence, or presentation. We have our turnPage() functionality and a few methods to provide different information about the book. All of the methods of the Book class are about business logic. So our perspective must be from the business's point of view. If our application is written to be used by real librarians who are searching for books and giving us a physical book, then SRP might be violated.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.SRPGoldLevel,
                    Description = "Gold level quiz for the Single Responsibility Principle",
                    Name = "Single Responsibility Principle Gold Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 80,
                    AchivementSystemName = SystemStudentAchievementNames.SRPGoldLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Gold

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallOCPQuizzes()
        {
            List<QuizAnswer> answers;
            List<QuizQuestion> questions;
            var quizzes = new List<Quiz>();

            #region Bronze

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Is the goal to allow classes to be easily extended to incorporate new behavior without modifying existing code.",
                    Hint = "The goal is to allow classes to be easily extended to incorporate new behavior without modifying existing code.",
                    Points = 10,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Extendable classes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "New behaviors can easily be added",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Existing code can be left unmodified",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following goals is achieved through the Open-Closed Principle?",
                Hint = "If we wanted to print a the Dinner menu items, we would need to change the existing PrintMenu method.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following snippet of code a violation of the Open-Closed Principle?",
                Hint = "The solution works. Only, a customer asks: “extending the AreaCalculator class to also calculate the area of triangles isn’t very hard, is it?”. Of course in this very basic scenario it isn’t but it does require us to modify the code. That is, AreaCalculator isn’t closed for modification as we need to change it in order to extend it. Or in other words: it isn’t open for extension.",
                Image = "OCP/OCPArea.png",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following class diagram in violation of the Open-Closed Principle?",
                Image = "OCP/UserLogic.png",
                Hint = "The User class uses the Logic class directly. If we need to implement a second Logic class in a way that will allow us to use both the current one and the new one, the existing Logic class will need to be changed. User is directly tied to the implementation of Logic, there is no way for us to provide a new Logic without affecting the current one. And when we are talking about statically typed languages, it is very possible that the User class will also require changes. If we are talking about compiled languages, most certainly both the User executable and the Logic executable or dynamic library will require recompilation and redeployment to our clients, a process we want to avoid whenever possible.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following class diagram in violation of the Open-Closed Principle?",
                Image = "OCP/InstrumentSpec.png",
                Hint = "InstrumentSpec is an abstract base class. It defines a matches() method that has a basic spec matching implementation.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.OCPBronzeLevel,
                    Description = "Bronze level quiz for the Open-Closed Principle",
                    Name = "Open-Closed Principle Bronze Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 40,
                    AchivementSystemName = SystemStudentAchievementNames.OCPBronzeLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Bronze

            #region Silver

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Should the Open-Closed Principle be applied everywhere?",
                    Hint = "Be careful when choosing the areas of code that need to be extended; applying the principle everywhere is wasteful and unnecessary, and can lead to complex, hard-to-understand code.",
                    Points = 20,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "abstract",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "override",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "virtual",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which keyword is used to define a method that can be overridden?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Less",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "More",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is your software more or less usable after implementing the Open-Closed Principle?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Interface Segregation Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which other SOLID design principle makes the Open-Closed Principle easier?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following diagram a better example of the Open-Closed Principle?",
                Image = "OCP/CallManager.png",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.OCPSilverLevel,
                    Description = "Silver level quiz for the Open-Closed Principle",
                    Name = "Open-Closed Principle Silver Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 60,
                    AchivementSystemName = SystemStudentAchievementNames.OCPSilverLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Silver

            #region Gold

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Is the following snippet of code a violation of the Open-Closed Principle?",
                    Image = "OCP/UpdateFontStyle.png",
                    Hint = "It is OK to violate the principle when the number of options in the if or switch statement is unlikely to change (e.g. switch on enum)",
                    Points = 30,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Encapsulation",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Abstraction",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Polymorphism",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Inheritance",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the basic Object-Oriented principles is a simple example of the Open-Closed Principle?",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Maybe",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Can global variables be used in the Open-Closed Principle",
                Hint = "No module that depends upon a global variable can be closed against any other module that might write to that variable. Any module that uses the variable in a way that the other modules don’t expect, will break those other modules. ",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Encapsulation/Inheritance",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Abstraction/Inheritance",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Encapsulation/Abstraction",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the basic Object-Oriented principles does the Open-Closed Principle combine?",
                Hint = "You are finding behavior that stays the same, and abstracting that behavior away into a base class, and then locking that code up from modification.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Does the following code snippet confirm to the Open-Closed Principle or not?",
                Image = "OCP/Shape.png",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.OCPGoldLevel,
                    Description = "Gold level quiz for the Open-Closed Principle",
                    Name = "Open-Closed Principle Gold Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 80,
                    AchivementSystemName = SystemStudentAchievementNames.OCPGoldLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Gold

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallLSPQuizzes()
        {
            List<QuizAnswer> answers;
            List<QuizQuestion> questions;
            var quizzes = new List<Quiz>();

            #region Bronze

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "A derived object may be treated as if it is the base object.",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "A derived object should be replaced by its base object.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Derived objects should be used instead of base objects.",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "The Liskov Substitution Principle is best described by which of the following?",
                    Points = 10,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Abstraction",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Encapsulation",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Inheritance",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Violating the Liskov Substitution Principle is a bad use of:",
                Hint = "When you use inheritance, your subclass gets all methods from its superclass, even if you don't want those methods.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Virtulaised",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Derivable",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Abstracted",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Substitutable",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Complete this sentence: Subtypes must be _____ for their base types",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.LSPBronzeLevel,
                    Description = "Bronze level quiz for the Liskov Substitution Principle",
                    Name = "Liskov Substitution Principle Bronze Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 33,
                    AchivementSystemName = SystemStudentAchievementNames.LSPBronzeLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Bronze

            #region Silver

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Someone wants to use an existing game system framework but with a 3D board as it will be based in the sky. As shown below, they take the existing Board base type and extend it to support their 3D board. Is this in violation of the Liskov Substitution Principle?",
                    Image = "LSP/Board.png",
                    Hint = "When 3DBoard subclasses Board, it inherits all of its methods but has no use for them.. Calling getUnits(2,5) for example doesn't make sense for 3DBoard.",
                    Points = 20,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Good",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Bad",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a good or bad example of the Liskov Substitution Principle?",
                Image = "LSP/Product.png",
                Hint = "Does a movie have an author?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Inheritance",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Complexity",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Unexpected results",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What should you avoid when implementing the Liskov Substitution Principle?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.LSPSilverLevel,
                    Description = "Silver level quiz for the Liskov Substitution Principle",
                    Name = "Liskov Substitution Principle Silver Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 66,
                    AchivementSystemName = SystemStudentAchievementNames.LSPSilverLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Silver

            #region Gold

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Delegate",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Composition",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both of the above",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Which of the following can be used instead of heritance to solve the problem of the Board superclass?",
                    Image = "LSP/Board.png",
                    Points = 30,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a good example of the Liskov Substitution Principle?",
                Image = "LSP/Bird.png",
                Hint = "What do you think would happen when this code is executed? As soon as an Ostrich instance is passed, it blows up!!! Here the sub type is not replaceable for the super type.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "A subclass that does not keep all the external observable behavior of it's parent class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A subclass that overrides a virtual method defined in it's parent class using an empty implementation in order to hide certain behavior defined in it's parent class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A subclass extends the external observable behavior of it's parent class.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A and B",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following are signs of Liskov Substitution Principle violations?",
                Hint = "C is wrong is we want to extend classes. If it had said 'A subclass modifies, rather than extends, the external observable behavior of it's parent class.', then it would have been wrong as well.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.LSPGoldLevel,
                    Description = "Gold level quiz for the Liskov Substitution Principle",
                    Name = "Liskov Substitution Principle Gold Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 100,
                    AchivementSystemName = SystemStudentAchievementNames.LSPGoldLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Gold

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallISPQuizzes()
        {
            List<QuizAnswer> answers;
            List<QuizQuestion> questions;
            var quizzes = new List<Quiz>();

            #region Bronze

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "That you must use interfaces",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Classes should implement all interfaces",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Clients should not be forced to implement interfaces they do not use",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Interfaces should be segregated away from implementation",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What does the Interface Segregation Principle state?",
                    Points = 10,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "True",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "False",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Interfaces communicate to the client code on how to use the module.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the Vehicle class shown below a good interface design?",
                Image = "ISP/VehicleInterface.png",
                Hint = "What would happen if the vehicle didn't have a CD player?",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.ISPBronzeLevel,
                    Description = "Bronze level quiz for the Interface Segregation Principle",
                    Name = "Interface Segregation Principle Bronze Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 33,
                    AchivementSystemName = SystemStudentAchievementNames.ISPBronzeLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Bronze

            #region Silver

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Implementations",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Clients",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Do interfaces belong to their implementations or clients?",
                    Points = 20,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Thin interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A medium interface",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A fat interface",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "If you implement an interface and have to throw an exception in a method because you don't support it, is the interface said to be a:",
                Hint = "A interface with to many methods is said to be a 'Fat interface'",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "The Single Responsibility Principle",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "The Open-Closed Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Liskov Substitution Principle",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "The Dependency Inversion Principle",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "To design an interface, what other SOLID design principle could be used?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.ISPSilverLevel,
                    Description = "Silver level quiz for the Interface Segregation Principle",
                    Name = "Interface Segregation Principle Silver Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 66,
                    AchivementSystemName = SystemStudentAchievementNames.ISPSilverLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Silver

            #region Gold

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "In the following code snippet, the interface IMachine is being implemented. If Machine is a photocopier, does the interface conform to the Interface Segregation Principle?",
                    Image = "ISP/Imachine.png",
                    Hint = "All methods relate to a photocopier, but could it be reused? What would happen if a photocopier without a stapler was forced to implement the method 'Staple'?",
                    Points = 30,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Visitor",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Iterator",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Template Method",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Coupling",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which other Object-Oriented principle does the Interface Segregation Principle help with?",
                Hint = "Interfaces decouple the implementation and so makes systems easier to refactor, change and redeploy.",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Don't use interfaces",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Split IProduct into three interfaces. Those being IDVD, IBluRay, ITShirt",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Create an IMovie interface",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Don't implement the IProduct interface in the Tshirt class",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Below is a snippet of code that is in violation of the Interface Segregation Principle. How could it be refactored to conform to the principle?",
                Image = "ISP/IProduct.png",
                Hint = "The into RunningTime is not required for a Tshirt and so this should be refactored. By creating an IMovie class that it can go into, The DVD and BluRay classes can both implement IProduct and IMovie while Tshirt only needs to implement IProduct",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 3

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.ISPGoldLevel,
                    Description = "Gold level quiz for the Interface Segregation Principle",
                    Name = "Interface Segregation Principle Gold Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 100,
                    AchivementSystemName = SystemStudentAchievementNames.ISPGoldLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Gold

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

        private void InstallDIPQuizzes()
        {
            List<QuizAnswer> answers;
            List<QuizQuestion> questions;
            var quizzes = new List<Quiz>();

            #region Bronze

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Abstractions",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "Classes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Neither",
                    IsCorrect = false
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you depend upon?",
                    Hint = "You should depend upon abstractions. Do not depend upon concrete classes",
                    Points = 10,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following diagram a good example of the Dependency Inversion Principle?",
                Image = "DIP/Pizza.png",
                Hint = "PizzaStore depends only on Pizza, the abstract class. Pizza is an abstract class..an abstraction. The concrete pizza classes depend on the Pizza abstraction too, because they implement the Pizza interface.",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Low level of coupling",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Easier to maintain systems",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "High level of coupling",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "High level of cohesion",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "What does not following the Dependency Inversion Principle cause",
                Hint = "Two classes are tightly coupled if they are linked together and are dependent on each other",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Layering",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Interfaces",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both of the above",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following avoids violating the Dependency Inversion Principle?",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Dependency Inversion Principle?",
                Image = "DIP/GetProductService.png",
                Hint = "The GetProductService class is relying on the ProductRepository class",
                Points = 10,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.DIPBronzeLevel,
                    Description = "Bronze level quiz for the Dependency Inversion Principle",
                    Name = "Dependency Inversion Principle Bronze Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 40,
                    AchivementSystemName = SystemStudentAchievementNames.DIPBronzeLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Bronze

            #region Silver

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Neither should depend on each other",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Should high-level components depend on low-level components?",
                    Points = 20,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "True",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "False",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following statement true or false: 'No class should derive from a concrete class'?",
                Hint = "If you override an implemented method, then your base class wasn't really an abstraction to start with. Those methods implemented in the base class are meant to be shared by all your subclasses.",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "High-level modules should not depend on low-level modules. Both should depend on abstractions.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Abstractions should not depend on details. Details should depend on abstractions.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "None of the above",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "A and B",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following are true?",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Without following the Dependency Inversion Principle, can stubs, mocks and fakes in unit tests be used?",
                Hint = "Stubs, mocks and fakes are only possible when we have an interface to implement",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Is the following code snippet a violation of the Dependency Inversion Principle?",
                Image = "DIP/SecurityService.png",
                Hint = "The use of 'static' is a violation as it is not abstracted away.",
                Points = 20,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.DIPSilverLevel,
                    Description = "Silver level quiz for the Dependency Inversion Principle",
                    Name = "Dependency Inversion Principle Silver Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 60,
                    AchivementSystemName = SystemStudentAchievementNames.DIPSilverLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Silver

            #region Gold

            #region Question 1

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Yes",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No",
                    IsCorrect = true
                }
            };

            questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Should we depend on concrete classes?",
                    Points = 30,
                    QuizAnswers = answers
                }
            };

            #endregion Question 1

            #region Question 2

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "It inverts the class diagram",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "It inverts the way we think about Object-Oriented design",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "It inverts the way the compiler compiles the code",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Where's the 'inversion' in Dependency Inversion Principle?",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 2

            #region Question 3

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "No variable should hold a reference to a concrete class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No class should derive from a concrete class",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "No method should override an implemented method of any of its base classes.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "All of the above",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following are guidelines to follow?",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 3

            #region Question 4

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Inverse the class diagram",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Constructor Injection",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Through the use of Inversion of Control",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "B and C",
                    IsCorrect = true
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "How can the Dependency Inversion Principle be enabled?",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 4

            #region Question 5

            answers = new List<QuizAnswer>
            {
                new QuizAnswer
                {
                    Text = "Entity objects should not have dependencies",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Static variables should be isolated.",
                    IsCorrect = false
                },
                new QuizAnswer
                {
                    Text = "Both of the above",
                    IsCorrect = true
                },
                new QuizAnswer
                {
                    Text = "None of the above.",
                    IsCorrect = false
                }
            };

            questions.Add(new QuizQuestion
            {
                Question = "Which of the following are true statements?",
                Points = 30,
                QuizAnswers = answers
            });

            #endregion Question 5

            quizzes.Add(
                new Quiz
                {
                    SystemName = SystemQuizNames.DIPGoldLevel,
                    Description = "Gold level quiz for the Dependency Inversion Principle",
                    Name = "Dependency Inversion Principle Gold Level Quiz",
                    CreatedUtc = DateTime.UtcNow,
                    IsSystemQuiz = false,
                    IsLevelQuiz = true,
                    IsStudentQuiz = false,
                    PassMark = 80,
                    AchivementSystemName = SystemStudentAchievementNames.DIPGoldLevel,
                    CreatedBy = administrativeUser,
                    QuizQuestions = questions
                }
            );

            #endregion Gold

            quizzes.ForEach(quiz => _quizRepository.Insert(quiz));
        }

    }
}
