using System;
using System.Collections.Generic;

namespace PRN222_Project.Models;

public partial class Screen
{
    public int ScreenId { get; set; }

    public int? CinemaId { get; set; }

    public string? ScreenName { get; set; }

    public int? Capacity { get; set; }

    public virtual Cinema? Cinema { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
