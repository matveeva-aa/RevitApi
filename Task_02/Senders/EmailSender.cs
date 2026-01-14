using System;

namespace Task_02.Senders
{
    public class EmailSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            Console.WriteLine($"Email для {recipient}: {message}");
        }
    }
}
