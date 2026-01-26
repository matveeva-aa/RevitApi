using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task_06
{
    [Transaction(TransactionMode.ReadOnly)]
    public class WallDistanceCommand : IExternalCommand
    {
        private const double Tol = 1e-6;
        private const string Title = "Расстояние между стенами";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            void Show(string text) => TaskDialog.Show(Title, text);

            IList<Reference> picked;
            try
            {
                picked = uiDoc.Selection.PickObjects(
                    ObjectType.Element,
                    new WallSelectionFilter(),
                    "Выберите две стены");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            if (picked.Count != 2)
            {
                Show("Нужно выбрать две стены");
                return Result.Cancelled;
            }

            Wall wall1 = doc.GetElement(picked[0]) as Wall;
            Wall wall2 = doc.GetElement(picked[1]) as Wall;

            XYZ n1 = GetWallNormal(wall1);
            XYZ n2 = GetWallNormal(wall2);

            if (n1 == null || n2 == null)
            {
                Show("Не удалось получить нормаль одной из стен, возможно она не прямая");
                return Result.Failed;
            }

            double dot = n1.DotProduct(n2);
            if (Math.Abs(Math.Abs(dot) - 1.0) > Tol)
            {
                Show("Стены не параллельны");
                return Result.Succeeded;
            }

            XYZ mid1 = GetWallMidpoint(wall1);
            XYZ mid2 = GetWallMidpoint(wall2);

            if (mid1 == null || mid2 == null)
            {
                Show("Не удалось получить точки для расчёта");
                return Result.Failed;
            }

            XYZ vectorBetween = mid2 - mid1;
            double distanceFt = Math.Abs(vectorBetween.DotProduct(n1));

            double distanceMm = UnitUtils.ConvertFromInternalUnits(distanceFt, DisplayUnitType.DUT_MILLIMETERS);
            int distanceMmInt = (int)Math.Round(distanceMm, 0, MidpointRounding.AwayFromZero);

            Show($"Расстояние: {distanceMmInt} мм");

            return Result.Succeeded;
        }

        private static XYZ GetWallNormal(Wall wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            if (location == null) return null;

            Curve curve = location.Curve;
            if (curve == null) return null;

            Line line = curve as Line;
            if (line == null) return null;

            XYZ start = line.GetEndPoint(0);
            XYZ end = line.GetEndPoint(1);

            XYZ direction = (end - start).Normalize();
            XYZ up = XYZ.BasisZ;

            return direction.CrossProduct(up).Normalize();
        }

        private static XYZ GetWallMidpoint(Wall wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            if (location == null) return null;

            Curve curve = location.Curve;
            if (curve == null) return null;

            Line line = curve as Line;
            if (line == null) return null;

            XYZ p0 = line.GetEndPoint(0);
            XYZ p1 = line.GetEndPoint(1);

            return (p0 + p1) / 2.0;
        }

        private class WallSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem) => elem is Wall;
            public bool AllowReference(Reference reference, XYZ position) => false;
        }
    }
}
