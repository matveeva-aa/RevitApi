using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task_07
{
    [Transaction(TransactionMode.Manual)]
    public class SystemFamilyGeometryStatsCommand : IExternalCommand
    {
        private const string Title = "Статистика системного семейства";
        private const double SolidVolumeEps = 1e-6;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Reference pick;
            try
            {
                pick = uiDoc.Selection.PickObject(
                    ObjectType.Element,
                    new SystemFamilyInstanceFilter(),
                    "Выберите экземпляр системного семейства");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            Element el = doc.GetElement(pick);
            if (el == null)
            {
                TaskDialog.Show(Title, "Не удалось получить выбранный элемент");
                return Result.Failed;
            }

            Options opt = new Options();
            GeometryElement geo = el.get_Geometry(opt);
            if (geo == null)
            {
                TaskDialog.Show(Title, "Геометрия элемента недоступна");
                return Result.Failed;
            }

            List<Solid> solids = geo
                .OfType<Solid>()
                .Where(s => s != null && s.Volume > SolidVolumeEps)
                .ToList();

            if (solids.Count == 0)
            {
                TaskDialog.Show(Title, "Solid-ы не найдены");
                return Result.Succeeded;
            }

            double totalVolumeFt3 = 0.0;
            double totalAreaFt2 = 0.0;
            int totalFaces = 0;
            int totalEdges = 0;
            double totalEdgeLenFt = 0.0;

            foreach (Solid s in solids)
            {
                totalVolumeFt3 += s.Volume;
                totalAreaFt2 += s.SurfaceArea;

                totalFaces += s.Faces?.Size ?? 0;
                totalEdges += s.Edges?.Size ?? 0;

                if (s.Edges != null)
                {
                    foreach (Edge e in s.Edges)
                    {
                        Curve c = e.AsCurve();
                        if (c != null) totalEdgeLenFt += c.Length;
                    }
                }
            }

            double volM3 = UnitUtils.ConvertFromInternalUnits(totalVolumeFt3, DisplayUnitType.DUT_CUBIC_METERS);
            double areaM2 = UnitUtils.ConvertFromInternalUnits(totalAreaFt2, DisplayUnitType.DUT_SQUARE_METERS);
            double lenM = UnitUtils.ConvertFromInternalUnits(totalEdgeLenFt, DisplayUnitType.DUT_METERS);

            var sb = new StringBuilder();
            sb.AppendLine($"Элемент: {el.Name} (Id {el.Id.IntegerValue})");
            sb.AppendLine($"Solid-ов: {solids.Count}");
            sb.AppendLine();
            sb.AppendLine($"Суммарный объём: {volM3:F6} м³");
            sb.AppendLine($"Суммарная площадь поверхности: {areaM2:F4} м²");
            sb.AppendLine($"Количество граней: {totalFaces}");
            sb.AppendLine($"Количество ребер: {totalEdges}");
            sb.AppendLine($"Суммарная длина ребер: {lenM:F3} м");

            TaskDialog.Show(Title, sb.ToString());
            return Result.Succeeded;
        }

        private class SystemFamilyInstanceFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                if (elem == null) return false;
                if (elem is ElementType) return false;
                if (elem is FamilyInstance) return false;

                return elem is Wall
                       || elem is Floor
                       || elem is Ceiling
                       || elem is RoofBase;
            }

            public bool AllowReference(Reference reference, XYZ position) => false;
        }
    }
}
