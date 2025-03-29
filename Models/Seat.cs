using System;
using System.Collections.Generic;

namespace PRN222_Project.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? ScreenId { get; set; }

    public string? SeatNumber { get; set; }

    public string? Status { get; set; }

    public virtual Screen? Screen { get; set; }

    public virtual ICollection<SeatBooking> SeatBookings { get; set; } = new List<SeatBooking>();
}
