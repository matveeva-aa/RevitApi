using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Task_08.Abstractions;
using Task_08.Services;

namespace Task_08.Services
{
    public class WallSelectionService : IWallSelectionService
    {
        private readonly IRevitContext _context;

        public WallSelectionService(IRevitContext context)
        {
            _context = context;
        }

        public Wall PickWall()
        {
            try
            {
                var r = _context.UiDoc.Selection.PickObject(
                    ObjectType.Element,
                    new WallSelectionFilter(),
                    "Выберите стену");

                return _context.Doc.GetElement(r) as Wall;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
        }
    }
}
