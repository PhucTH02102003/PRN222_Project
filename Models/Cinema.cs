using System;
using System.Collections.Generic;

namespace PRN222_Project.Models;

public partial class Cinema
{
    public int CinemaId { get; set; }

    public string CinemaName { get; set; } = null!;

    public string? Location { get; set; }

    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();
}
