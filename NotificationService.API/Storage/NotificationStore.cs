using Shared.Library.Models;

namespace NotificationService.API.Storage
{
    public static class NotificationStore
    {
        public static List<Notification> Notifications { get; set; }
        //    = new List<Notification>
        //{
        //    new Notification { Id = 1, Message = "Welcome to the system!", Status = "Read" },
        //    new Notification { Id = 2, Message = "Your appointment is confirmed.", Status = "Unread" },
        //    new Notification { Id = 3, Message = "Lab results are available.", Status = "Unread" }
        //};
    }
}
