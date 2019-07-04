using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDbTestConsole
{
    internal class InheritanceTest
    {
        private const string Collection = "inheriteData";

        internal void Run(MongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase(Program.Database);
            var collection = db.GetCollection<Motorized>(Collection);

            var bike = new StreetVehicle()
            {
                MotorType = "Yamaha",
                NumberOfWheels = 2
            };

            var car = new StreetVehicle()
            {
                MotorType = "Bmw",
                NumberOfWheels = 4
            };

            var aircraft = new FlyingVehicle()
            {
                MotorType = "Boing",
                WingType = "classic"
            };

            //collection.InsertOne(bike);
            //collection.InsertOne(car);
            //collection.InsertOne(aircraft);

            var test = collection.Find(Builders<Motorized>.Filter.Eq(e => e.MotorType, "Bmw"));
            var results = test.ToList();
            Console.WriteLine($"found {results.Count} motorized vehicles");
        }
    }

    [BsonKnownTypes(typeof(StreetVehicle))]
    [BsonKnownTypes(typeof(FlyingVehicle))]
    class Motorized
    {
        public ObjectId Id { get; set; }
        public string MotorType { get; set; }
    }

    class StreetVehicle : Motorized
    {
        public int NumberOfWheels { get; set; }
    }

    class FlyingVehicle : Motorized
    {
        public string WingType { get; set; }
    }
}