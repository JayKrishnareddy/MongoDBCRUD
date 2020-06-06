using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace MongoDBDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {

            MongoCRUD mongoCRUD = new MongoCRUD("Student");

            #region Insert Record
            //var model = new Model
            //{
            //    FirstName = "Mohit",
            //    LastName = "Ande",
            //    Age = 24,
            //    AddressModel = new AddressModel
            //    {
            //        State = "Telangana",
            //        City = "Hyderabad",
            //        Nationality = "Indian"
            //    }
            //};
            //mongoCRUD.InsertRecord("Test", model);
            #endregion

            #region  GetRecords
            //var record = mongoCRUD.GetRecords<Model>("Test");
            //foreach (var i in record)
            //{
            //    Console.WriteLine($" { i.Id}: {i.FirstName} {i.LastName} ");
            //}
            #endregion

            #region GetRecordbyId
            // dc90f7b8-98b3-463d-bcc9-43386cb99942 
            // 8e17bbd4-fdf8-4e75-956d-e91bc1989a0b
            //var rec = mongoCRUD.GetRecordById<Model>("Test", new Guid("8e17bbd4-fdf8-4e75-956d-e91bc1989a0b"));
            //Console.WriteLine(rec.FirstName);
            #endregion

            #region UpdateRecord
            //var result = mongoCRUD.GetRecordById<Model>("Test", new Guid("8e17bbd4-fdf8-4e75-956d-e91bc1989a0b"));
            //result.DateTime = new DateTime(1995, 11, 27,0,0,0, DateTimeKind.Utc);
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
            model.DateTime = new DateTime(1995, 11, 27,0,0,0, DateTimeKind.Utc);
            mongoCRUD.UpdateRecord("Test", model.Id, model);

            #endregion

            #region DeleteRecord
            //var res = mongoCRUD.GetRecordById<Model>("Test", new Guid("8e17bbd4-fdf8-4e75-956d-e91bc1989a0b"));
            //mongoCRUD.DeleteRecord<Model>("Test", res.Id);
            #endregion

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
        [BsonElement("Dob")]
        public DateTime DateTime { get; set; }
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

        public List<T> GetRecords<T>(string Table)
        {
            var collection = db.GetCollection<T>(Table);
            return collection.Find(new BsonDocument()).ToList();

        }

        public T GetRecordById<T>(string table, Guid Id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", Id);
            return collection.Find(filter).First();
        }

        
        public void UpdateRecord<T>(string table, Guid id , T record)
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid Id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", Id);
            collection.DeleteOne(filter);
        }
    }

}
