using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task_05
{
    [Transaction(TransactionMode.Manual)]
    public class SelectionStatsCommand : IExternalCommand
    {
        private const string NoSelectionMessage = "Элементы не выбраны";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Reference> pickedRefs = uiDoc.Selection.PickObjects(
                    ObjectType.Element,
                    new FamilyInstanceFilter(),
                    "Выберите экземпляры загружаемых семейств"
                );

                if (pickedRefs == null || pickedRefs.Count == 0)
                {
                    TaskDialog.Show("Статистика", NoSelectionMessage);
                    return Result.Succeeded;
                }

                var countsByCategory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (Reference r in pickedRefs)
                {
                    Element e = doc.GetElement(r);
                    if (e == null) continue;

                    string catName = e.Category?.Name ?? "<Без категории>";

                    if (countsByCategory.ContainsKey(catName))
                        countsByCategory[catName]++;
                    else
                        countsByCategory.Add(catName, 1);
                }

                int total = countsByCategory.Values.Sum();

                string report =
                    $"Всего элементов: {total}\n\n" +
                    string.Join("\n", countsByCategory
                        .OrderByDescending(kv => kv.Value)
                        .Select(kv => $"{kv.Key} - {kv.Value}"));

                TaskDialog.Show("Статистика", report);
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Статистика", NoSelectionMessage);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
                return Result.Failed;
            }
        }

        private class FamilyInstanceFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is FamilyInstance;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
    }
}
