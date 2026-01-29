using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Task_08.Abstractions
{
    public interface IRevitContext
    {
        UIApplication UiApp { get; }
        UIDocument UiDoc { get; }
        Document Doc { get; }
    }
}
