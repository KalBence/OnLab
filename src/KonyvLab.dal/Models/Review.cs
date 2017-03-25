using MongoDB.Bson;

namespace KonyvLab.dal.Models
{
    public class Review
    {
        public ObjectId _id { get; set; }
        public string UserName { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
    }
}
