
namespace Shared.Library.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
