using Task_02.Logging;
using Task_02.Senders;

namespace Task_02.Services
{
    public class NotificationService
    {
        private readonly INotificationSender _sender;
        private readonly ILogger _logger;

        public NotificationService(INotificationSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public void SendNotification(string message, string recipient)
        {
            string formattedMessage = $"Уведомление: {message}";

            _sender.Send(recipient, formattedMessage);

            _logger.Log($"Отправлено уведомление для {recipient} ({_sender.GetType().Name})");
        }
    }
}
