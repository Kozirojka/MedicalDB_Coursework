using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class HelpRequestStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MedicalHelpRequest> MedicalHelpRequests { get; set; } = new List<MedicalHelpRequest>();
}
