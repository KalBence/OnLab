using KonyvLab.dal.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonyvLab.dal.Managers
{
    public class MessageManager
    {

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected IMongoCollection<Message> _collection;

        public MessageManager()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("KonyvLab");
            _collection = _database.GetCollection<Message>("messages");
        }

        public void AddNewMessage(Message message)
        {
            _collection.InsertOne(message);
        }

        public IQueryable<Message> FindByUserName(string userName)
        {
            var q = from e in _collection.AsQueryable()
                    where e.ToUserName == userName
                    select e;
            return q;
        }



    }
}
