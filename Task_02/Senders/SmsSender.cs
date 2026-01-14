using System;

namespace Task_02.Senders
{
    public class SmsSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"SMS для {recipient}: {message}");
        }
    }
}
