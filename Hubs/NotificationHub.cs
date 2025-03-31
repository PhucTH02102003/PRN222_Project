using Microsoft.AspNetCore.SignalR;

namespace PRN222_Project.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
