using Microsoft.Extensions.DependencyInjection;
using Task_02.Logging;
using Task_02.Senders;
using Task_02.Services;

namespace Task_02.Composition
{
    public static class DependencyInjection
    {
        public static ServiceProvider Build(string channelChoice)
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILogger>(_ => new FileLogger("log.txt"));

            if (channelChoice == "2")
                services.AddTransient<INotificationSender, SmsSender>();
            else
                services.AddTransient<INotificationSender, EmailSender>();

            services.AddTransient<NotificationService>();

            return services.BuildServiceProvider();
        }
    }
}
