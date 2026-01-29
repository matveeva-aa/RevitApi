using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Task_08.Services
{
    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem is Wall;

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
