namespace T802.ServiceBusMessaging.Achievements
{
    public interface IAchievementMessagingService
    {
        void SendAchievementAwardedMessage(string username);
    }
}
