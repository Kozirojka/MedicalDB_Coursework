using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public decimal AvailableQuantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RequestMedicine> RequestMedicines { get; set; } = new List<RequestMedicine>();
}
