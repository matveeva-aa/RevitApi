using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Task_04
{
    [Transaction(TransactionMode.Manual)]
    public class WallStatsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            List<Wall> walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .OfType<Wall>()
                .ToList();

            if (walls.Count == 0)
            {
                TaskDialog.Show("Статистика стен", "В проекте нет стен");
                return Result.Succeeded;
            }

            double GetWallLengthFeet(Wall w)
            {
                Parameter p = w.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                return (p != null && p.HasValue) ? p.AsDouble() : 0.0;
            }

            var lengths = walls
                .Select(w => new { Wall = w, LenFt = GetWallLengthFeet(w) })
                .ToList();

            var valid = lengths.Where(x => x.LenFt > 0).ToList();
            if (valid.Count == 0)
            {
                TaskDialog.Show("Статистика стен", "Не удалось получить длины стен");
                return Result.Succeeded;
            }

            var max = valid.OrderByDescending(x => x.LenFt).First();
            var min = valid.OrderBy(x => x.LenFt).First();

            double sumFt = valid.Sum(x => x.LenFt);
            double avgFt = sumFt / valid.Count;

            double FtToM(double ft) =>
                UnitUtils.ConvertFromInternalUnits(ft, DisplayUnitType.DUT_METERS);

            string report =
                $"Всего стен: {walls.Count}\n" +
                $"Макс. длина: {FtToM(max.LenFt):F2} м\n" +
                $"Мин. длина: {FtToM(min.LenFt):F2} м\n" +
                $"Средняя длина: {FtToM(avgFt):F2} м";

            void SetComment(Wall w, string text)
            {
                Parameter p = w.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                if (p == null || p.IsReadOnly) return;
                p.Set(text);
            }

            using (Transaction t = new Transaction(doc, "Wall stats comments"))
            {
                t.Start();
                SetComment(max.Wall, "Самая длинная стена");
                SetComment(min.Wall, "Самая короткая стена");
                t.Commit();
            }

            TaskDialog.Show("Статистика стен", report);
            return Result.Succeeded;
        }
    }
}
