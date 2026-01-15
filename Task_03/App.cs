using Autodesk.Revit.UI;

namespace Task_03
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "ПИК";
            string panelName = "АР";

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch
            {
            }

            RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName);

            var buttonData = new PushButtonData(
                "OpeningsCommandButton",
                "Отверстия",
                typeof(App).Assembly.Location,
                "Task_03.OpeningsCommand"
            );

            panel.AddItem(buttonData);

            return Result.Succeeded;

        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
