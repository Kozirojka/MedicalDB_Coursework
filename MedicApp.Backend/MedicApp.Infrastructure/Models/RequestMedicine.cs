using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class RequestMedicine
{
    public int RequestMedicineId { get; set; }

    public int RequestId { get; set; }

    public int MedicineId { get; set; }

    public decimal QuantityRequired { get; set; }

    public decimal? QuantityProvided { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual MedicalHelpRequest Request { get; set; } = null!;
}
