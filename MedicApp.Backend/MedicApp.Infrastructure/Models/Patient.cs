using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Patient
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<MedicalHelpRequest> MedicalHelpRequests { get; set; } = new List<MedicalHelpRequest>();
}
