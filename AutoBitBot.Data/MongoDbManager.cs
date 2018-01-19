using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Data
{
    public class MongoDbManager
    {
        public async Task Run()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("autobitbot");
            var collection = database.GetCollection<BsonDocument>("bittrex");

            await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            var list = await collection.Find(new BsonDocument("Name", "Jack"))
                .ToListAsync();

            foreach (var document in list)
            {
                Console.WriteLine(document["Name"]);
            }
        }
    }
}
