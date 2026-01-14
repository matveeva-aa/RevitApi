using System;
using Task_02.Composition;
using Task_02.Services;

namespace Task_02
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Выберите тип уведомления: 1 - Email, 2 - SMS");
            string choice = Console.ReadLine();

            var provider = DependencyInjection.Build(choice);

            var service = (NotificationService)provider.GetService(typeof(NotificationService));

            Console.Write("Введите получателя: ");
            string recipient = Console.ReadLine();

            Console.Write("Введите сообщение: ");
            string message = Console.ReadLine();

            service.SendNotification(message, recipient);

            Console.WriteLine("Готово. Нажмите любую клавишу");
            Console.ReadKey();
        }
    }
}
