using Autodesk.Revit.DB;

namespace Task_08.Services
{
    public static class UnitConverter
    {
        public static double FtToMm(double feet)
            => UnitUtils.ConvertFromInternalUnits(feet, DisplayUnitType.DUT_MILLIMETERS);

        public static double Ft2ToM2(double squareFeet)
            => UnitUtils.ConvertFromInternalUnits(squareFeet, DisplayUnitType.DUT_SQUARE_METERS);

        public static double Ft3ToM3(double cubicFeet)
            => UnitUtils.ConvertFromInternalUnits(cubicFeet, DisplayUnitType.DUT_CUBIC_METERS);
    }
}