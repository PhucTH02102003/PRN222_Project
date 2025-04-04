﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Hubs;
using PRN222_Project.Models;

namespace PRN222_Project.Pages
{
    public class ManageBookingsModel : PageModel
    {
        private readonly CinemaManagementContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ManageBookingsModel(CinemaManagementContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IList<Booking> Bookings { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
        {
            var pageSize = 10;  // Chọn số lượng booking mỗi trang
            var totalBookings = await _context.Bookings.CountAsync();  // Tổng số booking

            // Tính số trang
            TotalPages = (int)Math.Ceiling(totalBookings / (double)pageSize);

            // Lấy dữ liệu với phân trang
            Bookings = await _context.Bookings
                .Include(b => b.User)  // Lấy thông tin khách hàng
                .Include(b => b.Showtime)  // Lấy thông tin showtime
                .Include(b => b.SeatBookings)
                    .ThenInclude(sb => sb.Seat)  // Lấy thông tin ghế
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            CurrentPage = pageIndex;  // Cập nhật trang hiện tại
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int bookingId)
        {
            // Lấy booking từ cơ sở dữ liệu
            var booking = await _context.Bookings
                .Include(b => b.User)  // Lấy thông tin người dùng (user)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound(); // Nếu không tìm thấy booking, trả về 404
            }

            // Chỉ thay đổi trạng thái nếu booking đang ở trạng thái Pending
            if (booking.Status == "Pending")
            {
                booking.Status = "Done";  // Thay đổi trạng thái thành Done
                _context.Bookings.Update(booking);  // Cập nhật booking
                await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu

                // Gửi thông báo đến người dùng qua SignalR
                var userId = booking.UserId.ToString();  // Lấy UserId của người dùng (giả sử UserId là kiểu int)
                var message = "Your booking has been completed and marked as 'Done'.";
                await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);  // Gửi thông báo
            }

            return RedirectToPage();  // Quay lại trang quản lý booking
        }
    }


}
