using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public DateOnly Date { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<ScheduleInterval> ScheduleIntervals { get; set; } = new List<ScheduleInterval>();
}
