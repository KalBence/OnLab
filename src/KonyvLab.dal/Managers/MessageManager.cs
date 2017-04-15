using KonyvLab.dal.Models;
using Microsoft.Extensions.Configuration;
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
        IConfiguration config;


        public MessageManager(IConfiguration config)
        {
            this.config = config;
            _client = new MongoClient(config["DbConnection"]);
            _database = _client.GetDatabase(config["DbName"]);
            _collection = _database.GetCollection<Message>(config["MessageTable"]);
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
