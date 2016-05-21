using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Autofac;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using T802.Core.Caching;
using T802.Core.Data;
using T802.Core.Domain.Students;
using T802.Data;
using T802.ServiceBusMessaging.Achievements;
using T802.Services.Achievements;
using T802.Services.Students;

namespace T802.WorkerRoleAchievementAwarder
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
#if DEBUG
        const string QueueName = "oodtutor-stage-achievements";
#else
        const string QueueName = "oodtutor-prod-achievements";
#endif

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        private IContainer _container;
        private IAchievementAwarder _achievementAwarder;
        private IStudentService _studentService;

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Process the message
                        Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
                        _achievementAwarder.AwardAchievements(_studentService.GetStudentByUsername(receivedMessage.GetBody<string>()));
                    }
                    catch
                    {
                        // Handle any message processing specific exceptions here
                    }
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            ConfigureDependencyInjection();

            _achievementAwarder = _container.Resolve<IAchievementAwarder>();
            _studentService = _container.Resolve<IStudentService>();

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }

        private void ConfigureDependencyInjection()
        {
            // Autofac configuration
            var builder = new ContainerBuilder();

            // Register your dependencies
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<T802Context>().As<IDbContext>().InstancePerLifetimeScope(); ;

            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance(); ;
            builder.RegisterType<AchievementService>().As<IAchievementService>();
            builder.RegisterType<StudentService>().As<IStudentService>();

            builder.RegisterType<AchievementMessagingService>().As<IAchievementMessagingService>();

            builder.RegisterType<AchievementAwarder>().As<IAchievementAwarder>();

            _container = builder.Build();
        }
    }
}
