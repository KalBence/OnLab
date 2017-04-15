using KonyvLab.dal.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonyvLab.dal.Managers
{

    public class ReviewManager
    {

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected IMongoCollection<Review> _collection;
        IConfiguration config;

        public ReviewManager(IConfiguration config)
        {
            this.config =  config;
            _client = new MongoClient(config["DbConnection"]);
            _database = _client.GetDatabase(config["DbName"]);
            _collection = _database.GetCollection<Review>(config["ReviewTable"]);           
        }

        public void AddNewReview(Review review)
        {
            review.ViewCount = 1;
            _collection.InsertOne(review);
        }

        public void IncreaseViewCount(string Id)
        {
            ObjectId oid = new ObjectId(Id);
            _collection.FindOneAndUpdate(Builders<Review>.Filter.Eq(nameof(Review._id), oid), Builders<Review>.Update.Inc(nameof(Review.ViewCount), 1));
        }

        public IQueryable<Review> GetTopReviews()
        {
            var q = (from e in _collection.AsQueryable().OrderByDescending(e => e.ViewCount)
                    select e).Take(10);
            return q;
        }

        public Review FindById(string Id)
        {
            ObjectId oid = new ObjectId(Id);
            Review p = _collection.Find(Builders<Review>.Filter.Eq(nameof(Review._id), oid)).FirstOrDefault();
            return p;
        }

        public IQueryable<Review> FindByUserName(string userName)
        {
            var q = from e in _collection.AsQueryable()
                    where e.UserName == userName
                    select e;
            return q;
        }

        public void Update(Review r)
        {
            _collection.FindOneAndUpdate(Builders<Review>.Filter.Eq(nameof(Review._id), r._id), Builders<Review>.Update
                .Set(nameof(Review.Title), r.Title).Set(nameof(Review.Content), r.Content).Set(nameof(Review.Author), r.Author).Set(nameof(Review.Rating), r.Rating));
        }

        public void Delete(string id)
        {
            ObjectId oid = new ObjectId(id);
            _collection.DeleteOne(Builders<Review>.Filter.Eq(nameof(Review._id), oid));
        }

        public IQueryable<Review> FindByTitle(string title)
        {
            var q = from e in _collection.AsQueryable()
                    where e.Title.ToUpper().Contains(title.ToUpper())
                    select e;
            return q;
        }

        public IQueryable<Review> FindByAuthor(string author)
        {
            var q = from e in _collection.AsQueryable()
                    where e.Author.ToUpper().Contains(author.ToUpper())
                    select e;
            return q;
        }

    }
}
