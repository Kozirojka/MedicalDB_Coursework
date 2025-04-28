using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Address
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string? Street { get; set; }

    public string? Building { get; set; }

    public string? Appartaments { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public double? Longitude { get; set; }

    public double? Latitude { get; set; }

    public string? Region { get; set; }

    public virtual Account Account { get; set; } = null!;
}
