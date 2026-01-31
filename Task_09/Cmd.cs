using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Task_09.Abstractions;
using Task_09.Services;
using Task_09.Views;
using Task_09.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Task_09
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<RevitTask>(new RevitTask());
            services.AddSingleton<ISelectionService, SelectionService>();
            services.AddSingleton<ISectionService, SectionService>();
            services.AddTransient<MainWindowViewModel, MainWindowViewModel>();
            services.AddTransient<MainWindow, MainWindow>();
            var provider = services.BuildServiceProvider();
            
            var mainWindow = provider.GetRequiredService<MainWindow>();
               
            mainWindow.Show();
            
            return Result.Succeeded;
        }
    }
}
