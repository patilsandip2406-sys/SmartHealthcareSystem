using Microsoft.AspNetCore.SignalR;
using NotificationService.API.Storage;
using Shared.Library.Models;

namespace NotificationService.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            try
            {
                var notification = new Notification
                {
                    Id = NotificationStore.Notifications != null ? NotificationStore.Notifications.Count + 1 : 1,
                    Message = message,
                    User = user,
                    Status = "Unread",
                    CreatedAt = DateTime.UtcNow
                };

                NotificationStore.Notifications.Add(notification);

                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                var res = ex.Message;
                throw;
            }
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
