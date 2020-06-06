using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using MongoDB.Driver;
namespace MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoCRUD mongoCRUD = new MongoCRUD("Student");
            var model = new Model
            {
                FirstName = "Mohit",
                LastName = "Ande",
                Age = 24,
                AddressModel = new AddressModel
                {
                    State = "Telangana",
                    City = "Hyderabad",
                    Nationality = "Indian"
                }
            };
            mongoCRUD.InsertRecord("Test", model);
        }
       
    }
    public class Model
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId] 
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public AddressModel AddressModel { get; set; }
    }

    public class AddressModel
    {
        public string Nationality { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
    public class MongoCRUD
    {
        private readonly IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        public void  InsertRecord<T> (string Table ,T record)
        {
            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);
        }
    }

}
