using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class ScheduleInterval
{
    public int Id { get; set; }

    public int ScheduleId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual ICollection<MedicalHelpRequest> MedicalHelpRequests { get; set; } = new List<MedicalHelpRequest>();

    public virtual Schedule Schedule { get; set; } = null!;
}
