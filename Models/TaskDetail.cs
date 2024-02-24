using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DirWatcher.Models
{
    public class TaskDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int TaskNo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TotalRunTime { get; set; }

        public int MagicCount { get; set; }

        public string? Status { get; set; }

        public List<string>? CurrentFiles { get; set; }

        public List<string>? AddedFiles { get; set; }

        public List<string>? DeletedFiles { get; set; }
    }
}