namespace Task_08.Models
{
    public class WallInfo
    {
        public string WallName { get; set; }
        public string WallType { get; set; }

        public double LengthMm { get; set; }
        public double HeightMm { get; set; }
        public double ThicknessMm { get; set; }

        public double VolumeM3 { get; set; }
        public double AreaM2 { get; set; }
    }
}
