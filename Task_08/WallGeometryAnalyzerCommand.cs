using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Task_08.Abstractions;
using Task_08.Services;
using Task_08.ViewModels;
using Task_08.Views;


namespace Task_08
{
        [Transaction(TransactionMode.ReadOnly)]
        public class WallGeometryAnalyzerCommand : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
            {
                try
                {
                    var services = new ServiceCollection();

                    services.AddSingleton<IRevitContext>(new RevitContext(commandData.Application));

                    services.AddSingleton<IWallSelectionService, WallSelectionService>();
                    services.AddSingleton<IWallGeometryService, WallGeometryService>();

                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<MainWindow>();

                    var provider = services.BuildServiceProvider();

                    var window = provider.GetRequiredService<MainWindow>();
                    window.Show();

                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                    return Result.Failed;
                }
            }
        }
}
