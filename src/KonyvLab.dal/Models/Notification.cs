using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace KonyvLab.dal.Models
{
    public class Notification
    {
        public ObjectId _id { get; set; }
        public ObjectId UserId { get; set; }
        public string Message { get; set; }
        public ObjectId Object { get; set; }
        public DateTime Time { get; set; }
        public bool WasRead { get; set; }
    }
}
