using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;
using Task_10.Abstractions;
using Task_10.Services;
using Task_10.ViewModels;
using Task_10.Views;

namespace Task_10
{
    internal class Config : ICommandConfiguration
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IPlacementService, PlacementService>();
            services.AddTransient<MainWindowViewModel, MainWindowViewModel>();
            services.AddTransient<MainWindow, MainWindow>();
        }
    }
}
