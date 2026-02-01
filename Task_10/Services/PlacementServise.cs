using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Task_10.Abstractions;
using Task_10.Models;

namespace Task_10.Services
{
    public class PlacementService : IPlacementService
    {
        private readonly Document _document;
        private Document document;

        public PlacementService(Document document)
        {
            _document = document;
        }

        public Result Place(TreeType treeType, int count)
        {
            return Validate(count)
                 .Bind(() => FindFamily(treeType))
                 .Bind(s => PlaceInstances(s, count));
        }

        private Result Validate(int count)
        {
            if (count < 0)
                return Result.Failure("Количество должно быть больше 0");
            return Result.Success();
        }

        private Result<FamilySymbol> FindFamily(TreeType treeType)
        {
            string treeName = string.Empty;
            switch (treeType)
            {
                case TreeType.Birch:
                    treeName = "Береза";
                    break;
                case TreeType.Apple:
                    treeName = "Яблоня";
                    break;
                case TreeType.Cherry:
                    treeName = "Вишня";
                    break;
            }

            FamilySymbol familySymbol = new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_Planting)
                .OfClass(typeof(FamilySymbol))
                .OfType<FamilySymbol>()
                .Where(x => x.FamilyName.Contains(treeName))
                .FirstOrDefault();

            if (familySymbol == null)
                return Result.Failure<FamilySymbol>("Не найден типоразмер для размещения");

            return familySymbol;
        }

        private Result PlaceInstances(FamilySymbol familySymbol, int count)
        {
            try
            {
                double step = UnitUtils.ConvertToInternalUnits(2, DisplayUnitType.DUT_METERS);
                    int cols = (int)Math.Ceiling(Math.Sqrt(count));
                    var points = new List<XYZ>(count);

                    for (int i = 0; i < count; i++)
                    {
                        int row = i / cols;
                        int col = i % cols;
                        points.Add(new XYZ(col * step, row * step, 0));
                    }

                var level = new FilteredElementCollector(_document)
                    .OfClass(typeof(Level))
                    .OfType<Level>()
                    .OrderBy(l => l.Elevation)
                    .FirstOrDefault();
                if (level == null)
                    return Result.Failure("Не удалось определить уровень для размещения");

                using (Transaction transaction = new Transaction(_document, "Размещение деревьев"))
                {
                    transaction.Start();
                    foreach (var point in points)
                    {
                        _document.Create.NewFamilyInstance(
                            point,
                            familySymbol,
                            level,
                            Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    transaction.Commit();
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            
            }
        }
    }

}
