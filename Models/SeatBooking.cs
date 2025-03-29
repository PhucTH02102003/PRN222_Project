using System;
using System.Collections.Generic;

namespace PRN222_Project.Models;

public partial class SeatBooking
{
    public int SeatBookingId { get; set; }

    public int? BookingId { get; set; }

    public int? SeatId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Seat? Seat { get; set; }
}
