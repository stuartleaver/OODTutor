using Nop.Services.Security;
using T802.Core;
using T802.Core.Caching;
using T802.Core.Data;
using T802.Data;
using T802.ServiceBusMessaging.Achievements;
using T802.Services.Achievements;
using T802.Services.Authentication;
using T802.Services.Installation;
using T802.Services.Leaderboards;
using T802.Services.Logging;
using T802.Services.Quizzes;
using T802.Services.Security;
using T802.Services.Students;
using T802.Web.Framework;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(T802.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(T802.Web.App_Start.NinjectWebCommon), "Stop")]

namespace T802.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // HTTpContextBase
            kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            // EF Context
            kernel.Bind<IDbContext>().To<T802Context>().InSingletonScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>)).Named("EfRepository");

            // Installation Service
            kernel.Bind<IInstallationService>().To<InstallationService>();

            // Student services
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<IStudentRegistrationService>().To<StudentRegistrationService>();

            // Achievement Service
            kernel.Bind<IAchievementService>().To<AchievementService>();

            // Security Services
            kernel.Bind<IEncryptionService>().To<EncryptionService>();

            // Authentication Service
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();

            // Activity Services
            kernel.Bind<IStudentActivityService>().To<StudentActivityService>();

            // Caching
            kernel.Bind<ICacheManager>().To<MemoryCacheManager>();

            // Work Context
            kernel.Bind<IWorkContext>().To<WebWorkContext>();

            // Web Helper
            kernel.Bind<IWebHelper>().To<WebHelper>();

            // Quizzes
            kernel.Bind<IQuizService>().To<QuizService>();
            kernel.Bind<IQuizResultService>().To<QuizResultService>();

            // Leaderboard
            kernel.Bind<ILeaderboardService>().To<LeaderboardService>();

            // Messaging
            kernel.Bind<IAchievementMessagingService>().To<AchievementMessagingService>();
        }
    }
}
