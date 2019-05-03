using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoDbTestConsole
{
    public class GeoLocation
    {
        private const string Collection = "geotest";
        private const string Category = "deleteme";

        public void Run(MongoClient client)
        {
            var db = client.GetDatabase(Program.Database);
            var collection = db.GetCollection<MyGeoModel>(Collection);

            collection.DeleteMany(Builders<MyGeoModel>.Filter.Eq(m => m.Category, Category));

            collection.Indexes.CreateOne(Builders<MyGeoModel>.IndexKeys.Geo2DSphere(m => m.Location));

            var entries = new List<MyGeoModel>()
            {
                new MyGeoModel
                {
                    CreatedAt = DateTime.UtcNow,
                    Category = Category,
                    Name = "some thing anywhere",
                    Location = GeoJson.Point(GeoJson.Geographic(11.514456, 48.148798))
                },
                new MyGeoModel
                {
                    CreatedAt = DateTime.UtcNow,
                    Category = Category,
                    Name = "some other thing",
                    Location = GeoJson.Point(GeoJson.Geographic(11.608875, 48.125127))
                },
                new MyGeoModel
                {
                    CreatedAt = DateTime.UtcNow,
                    Category = Category,
                    Name = "a thing",
                    Location = GeoJson.Point(GeoJson.Geographic(11.526979, 48.100626))
                }
            };

            collection.InsertMany(entries);

            var center = GeoJson.Point(GeoJson.Geographic(11.5411377, 48.1431816));

            var results = collection.Find(Builders<MyGeoModel>.Filter.Near(m => m.Location, center, 10000)).ToList();

            // currently it seems there is no C# function available to get the calculated distance
            // so here you go: https://stackoverflow.com/questions/30870654/c-sharp-mongodb-driver-2-0-getting-distance-back-from-near-query

            var geoNearOptions = new BsonDocument {
                { "near", new BsonDocument {
                    { "type", "Point" },
                    { "coordinates", new BsonArray { 11.5411377, 48.1431816 } },
                    } },
                { "distanceField", "dist.calculated" },
                { "maxDistance", 10000 },
                { "includeLocs", "Location" },
                { "num", 5 },
                { "spherical" , true }
            };

            var pipeline = new List<BsonDocument>();
            pipeline.Add(new BsonDocument { { "$geoNear", geoNearOptions } });

            var res = collection.Aggregate<BsonDocument>(pipeline).ToList();


            Console.WriteLine($"found {res.Count} in range");
            foreach (var result in res)
            {
                Console.WriteLine($"{result["Name"]} - distance to center: {result["dist"]["calculated"]}");
            }

            Console.ReadLine();
        }
    }

    public class MyGeoModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
    }
}
