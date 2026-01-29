using Autodesk.Revit.DB;

namespace Task_08.Abstractions
{
    public interface IWallSelectionService
    {
        Wall PickWall();
    }
}
