using Autodesk.Revit.DB;
using Task_08.Abstractions;
using Task_08.Models;

namespace Task_08.Services
{
    public class WallGeometryService : IWallGeometryService
    {
        public WallInfo GetWallInfo(Wall wall)
        {
            if (wall == null) return null;

            var info = new WallInfo
            {
                WallName = wall.Name,
                WallType = wall.WallType?.Name ?? "—",
                LengthMm = GetLengthMm(wall),
                HeightMm = GetHeightMm(wall),
                ThicknessMm = GetThicknessMm(wall),
                VolumeM3 = GetVolumeM3(wall),
                AreaM2 = GetAreaM2(wall)
            };

            return info;
        }

        private static double GetLengthMm(Wall wall)
        {
            var lc = wall.Location as LocationCurve;
            var lengthFt = lc?.Curve?.Length ?? 0.0;
            return UnitConverter.FtToMm(lengthFt);
        }

        private static double GetHeightMm(Wall wall)
        {
            var p = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
            if (p != null && p.StorageType == StorageType.Double)
                return UnitConverter.FtToMm(p.AsDouble());

            var bb = wall.get_BoundingBox(null);
            if (bb == null) return 0.0;

            return UnitConverter.FtToMm(bb.Max.Z - bb.Min.Z);
        }

        private static double GetThicknessMm(Wall wall)
        {
            var widthFt = wall.WallType?.Width ?? 0.0;
            return UnitConverter.FtToMm(widthFt);
        }

        private static double GetVolumeM3(Wall wall)
        {
            var p = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
            if (p == null || p.StorageType != StorageType.Double) return 0.0;
            return UnitConverter.Ft3ToM3(p.AsDouble());
        }

        private static double GetAreaM2(Wall wall)
        {
            var p = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
            if (p == null || p.StorageType != StorageType.Double) return 0.0;
            return UnitConverter.Ft2ToM2(p.AsDouble());
        }
    }
}
