using System;
using System.Collections.Generic;

namespace PRN222_Project.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int? ShowtimeId { get; set; }

    public DateTime? BookingTime { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<SeatBooking> SeatBookings { get; set; } = new List<SeatBooking>();

    public virtual Showtime? Showtime { get; set; }

    public virtual User? User { get; set; }
}
