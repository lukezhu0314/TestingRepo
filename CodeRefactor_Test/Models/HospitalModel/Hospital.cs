namespace CodeRefactor_Test.Models.HospitalModel
{
    public class Hospital
    {
        public string name { get; set; }
        public HospitalType type { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string coords { get; set; }
        public double DTN { get; set; }
    }

    public enum HospitalType
    {
        CSC,
        PSC
    }
}