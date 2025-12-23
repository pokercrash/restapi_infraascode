using Amazon.DynamoDBv2.DataModel;

namespace NoteApplication.Models
{
    [DynamoDBTable("notes")]
    public class Note
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
