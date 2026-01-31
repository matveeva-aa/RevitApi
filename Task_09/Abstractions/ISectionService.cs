using Autodesk.Revit.DB;

namespace Task_09.Abstractions
{
    public interface ISectionService
    {
        bool CreateSection(FamilyInstance familyInstance, double widthOffsetMm, double depthOffsetMm, double heightOffsetMm, string sectionName, string sectionName2, string sectionName3);
    }
}
