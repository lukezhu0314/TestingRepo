using MongoDB.Bson;

namespace CodeRefactor_Test.Models.TimeModel
{
    public class TravelTime
    {
        public ObjectId _id { get; set; }
        public int? DnS_PscTime { get; set; }
        public int? DnS_Psc_CscTime { get; set; }
        public int? MS_CscTime { get; set; }
    }
}