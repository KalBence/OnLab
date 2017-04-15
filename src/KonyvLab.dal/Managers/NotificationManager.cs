using KonyvLab.dal.Models;
using Microsoft.Extensions.Configuration;
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
        IConfiguration config;


        public NotificationManager(IConfiguration config)
        {
            this.config = config;
            _client = new MongoClient(config["DbConnection"]);
            _database = _client.GetDatabase(config["DbName"]);
            _collection = _database.GetCollection<Notification>(config["NotificationTable"]);
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

        public void ReadNotifications(String UserId)
        {
            var notifications = FindByUserId(UserId);
            foreach (var n in notifications)
                _collection.FindOneAndUpdate(Builders<Notification>.Filter.Eq(nameof(Notification._id), n._id), Builders<Notification>.Update
                .Set(nameof(Notification.WasRead), true));
        }

        


    }
}
