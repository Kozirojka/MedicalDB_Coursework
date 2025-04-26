using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class MedicalHelpRequest
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public int PatientId { get; set; }

    public int StatusId { get; set; }

    public int? DoctorId { get; set; }

    public int? ScheduleIntervalId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<RequestMedicine> RequestMedicines { get; set; } = new List<RequestMedicine>();

    public virtual ScheduleInterval? ScheduleInterval { get; set; }

    public virtual HelpRequestStatus Status { get; set; } = null!;
}
