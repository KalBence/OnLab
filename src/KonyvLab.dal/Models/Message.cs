using MongoDB.Bson;

namespace KonyvLab.dal.Models
{
    public class Message
    {
        public ObjectId _id { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Content { get; set; }


    }
}
