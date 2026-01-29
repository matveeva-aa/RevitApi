using Autodesk.Revit.DB;
using System.Dynamic;
using Task_08.Models;

namespace Task_08.Abstractions
{
    public interface IWallGeometryService
    {
        WallInfo GetWallInfo(Wall wall);
    }
}
