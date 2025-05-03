using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<MedicalHelpRequest> MedicalHelpRequests { get; set; } = new List<MedicalHelpRequest>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
