using System;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Task_09.Abstractions;

namespace Task_09.Services
{
    internal class SectionService : ISectionService
    {
        private readonly ExternalCommandData _commandData;

        public SectionService(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }

        public bool CreateSection(
            FamilyInstance instance,
            double widthOffsetMm,
            double depthOffsetMm,
            double heightOffsetMm,
            string sectionName,
            string sectionName2,
            string sectionName3)
        {

            var doc = _commandData.Application.ActiveUIDocument.Document;

            var bbox = instance.get_BoundingBox(null);

            if (bbox == null)
            {
                return false;
            }

            var center = (bbox.Min + bbox.Max) / 2;
            var size = bbox.Max - bbox.Min;

            Transform transform = Transform.CreateTranslation(XYZ.Zero);
            transform.Origin = center;
            transform.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transform.BasisY = XYZ.BasisZ;
            transform.BasisZ = XYZ.BasisY;

            Transform transform2 = Transform.CreateTranslation(XYZ.Zero);
            transform2.Origin = center;
            transform2.BasisX = (XYZ.BasisY.CrossProduct(XYZ.BasisZ)).Normalize();
            transform2.BasisY = XYZ.BasisY;
            transform2.BasisZ = XYZ.BasisZ;

            Transform transform3 = Transform.CreateTranslation(XYZ.Zero);
            transform3.Origin = center;
            transform3.BasisX = (XYZ.BasisY.CrossProduct(XYZ.BasisX)).Normalize();
            transform3.BasisY = XYZ.BasisY;
            transform3.BasisZ = XYZ.BasisX;

            var widthOffset = UnitUtils.ConvertToInternalUnits(widthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var depthOffset = UnitUtils.ConvertToInternalUnits(depthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var heightOffset = UnitUtils.ConvertToInternalUnits(heightOffsetMm, DisplayUnitType.DUT_MILLIMETERS);

            var sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;
            sectionBox.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            sectionBox.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            var sectionBox2 = new BoundingBoxXYZ();
            sectionBox2.Transform = transform2;
            sectionBox2.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            sectionBox2.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            var sectionBox3 = new BoundingBoxXYZ();
            sectionBox3.Transform = transform3;
            sectionBox3.Min = new XYZ(-size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset, -size.X / 2 - widthOffset);
            sectionBox3.Max = new XYZ(size.Z / 2 + depthOffset, size.Y / 2 + heightOffset, size.X / 2 + widthOffset);

            var viewType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Section);
            if (viewType == null )
            {
                return false;
            }

            try
            {
                using (var transaction = new Transaction(doc, "CreateSection"))
                {
                    transaction.Start();

                    var viewSection = ViewSection.CreateSection(doc, viewType.Id, sectionBox);
                    viewSection.Name = sectionName;
                    var viewSection2 = ViewSection.CreateSection(doc, viewType.Id, sectionBox2);
                    viewSection2.Name = sectionName2;
                    var viewSection3 = ViewSection.CreateSection(doc, viewType.Id, sectionBox3);
                    viewSection3.Name = sectionName3;

                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
