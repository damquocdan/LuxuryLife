using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class TourSchedule
{
    public int ScheduleId { get; set; }

    public int? TourId { get; set; }

    public int? Day { get; set; }

    public string? ActivityDescription { get; set; }

    public string? Location { get; set; }

    public DateOnly? ScheduleDate { get; set; }

    public virtual Tour? Tour { get; set; }
}
