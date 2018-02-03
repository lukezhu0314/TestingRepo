using System;
using MongoDB.Driver;
using MongoDB.Bson;
namespace MongoDB_Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("PreliminaryTravelTime");
            //var collection = db.GetCollection<TravelTime>("TravelTime");
            var collec = db.GetCollection<BsonDocument>("employee");
            var document = new BsonDocument{
                {"name", "Alex"},
                {"age", "28"}
            };
            collec.InsertOne(document);
            //Console.Read();
            //collection.InsertOneAsync(new TravelTime{Second = 100});
        }
    }
}
