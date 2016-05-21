using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;

namespace T802.ServiceBusMessaging.Achievements
{
    public class AchievementMessagingService : IAchievementMessagingService
    {
        // The name of your queue
#if DEBUG
        const string QueueName = "oodtutor-stage-achievements";
#else
        const string QueueName = "oodtutor-prod-achievements";
#endif

        private readonly string connectionString;
        private readonly QueueClient client;

        public AchievementMessagingService()
        {
            connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.Achievements.ConnectionString"];

            client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
        }
        public void SendAchievementAwardedMessage(string username)
        {
            var message = new BrokeredMessage(username);
            client.Send(message);
        }
    }
}
