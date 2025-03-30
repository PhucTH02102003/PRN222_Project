using Microsoft.AspNetCore.SignalR;

namespace PRN222_Project.Hubs
{
    public class BookingHub : Hub
    {
        // Phương thức này sẽ được gọi từ client khi một ghế được đặt
        public async Task NotifySeatBooked(int seatId)
        {
            // Gửi thông báo cho tất cả các client
            await Clients.All.SendAsync("ReceiveSeatUpdate", seatId);
        }
    }

}
