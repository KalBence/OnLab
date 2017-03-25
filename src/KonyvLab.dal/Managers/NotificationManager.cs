using KonyvLab.dal.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KonyvLab.dal.Managers
{
    public class NotificationManager
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected IMongoCollection<Notification> _collection;

        public NotificationManager()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("KonyvLab");
            _collection = _database.GetCollection<Notification>("notifications");
        }

        public void AddNewNotification(Notification notification)
        {
            _collection.InsertOne(notification);
        }

        public IQueryable<Notification> FindByUserId(String Id)
        {
            ObjectId oId = new ObjectId(Id);
            var q = from e in _collection.AsQueryable()
                    where e.UserId == oId
                    select e;
            return q;
        }



    }
}
