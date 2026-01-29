using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Task_08.Abstractions;

namespace Task_08.Services
{
    public class RevitContext : IRevitContext
    {
        public RevitContext(UIApplication uiApp)
        {
            UiApp = uiApp;
        }

        public UIApplication UiApp { get; }
        public UIDocument UiDoc => UiApp.ActiveUIDocument;
        public Document Doc => UiDoc.Document;
    }
}
