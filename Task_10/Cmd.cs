using Autodesk.Revit.Attributes;
using RxBim.Command.Revit;
using RxBim.Shared;
using System;
using Microsoft.Extensions.DependencyInjection;
using Task_10.Views;

namespace Task_10
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : RxBimCommand
    {
        public PluginResult ExecuteCommand(IServiceProvider provider)
        {
            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.ShowDialog();
            return PluginResult.Succeeded;
        }
    }
}