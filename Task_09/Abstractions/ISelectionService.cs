using Autodesk.Revit.DB;

namespace Task_09.Abstractions
{
    public interface ISelectionService
    {
        FamilyInstance PickFamilyInstance();
    }
}