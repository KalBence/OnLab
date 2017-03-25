using KonyvLab.dal.Models;
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

        public ReviewManager()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("KonyvLab");
            _collection = _database.GetCollection<Review>("reviews");
        }

        public void AddNewReview(Review review)
        {
            review.ViewCount = 1;
            _collection.InsertOne(review);
        }

        public void IncreaseViewCount(string Id)
        {
            ObjectId oid = new ObjectId(Id);
            _collection.FindOneAndUpdate(Builders<Review>.Filter.Eq("_id", oid), Builders<Review>.Update.Inc("ViewCount", 1));
        }

        public IQueryable<Review> GetAllReviews()
        {
            var q = from e in _collection.AsQueryable()
                    select e;
            return q.OrderByDescending(e => e.ViewCount);
        }

        public Review FindById(string Id)
        {
            ObjectId oid = new ObjectId(Id);
            Review p = _collection.Find(Builders<Review>.Filter.Eq("_id", oid)).FirstOrDefault();
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
            _collection.FindOneAndUpdate(Builders<Review>.Filter.Eq("_id", r._id), Builders<Review>.Update
                .Set("Title", r.Title).Set("Content", r.Content).Set("Author", r.Author).Set("Rating", r.Rating));
        }

        public void Delete(string id)
        {
            ObjectId oid = new ObjectId(id);
            _collection.DeleteOne(Builders<Review>.Filter.Eq("_id", oid));
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
