using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Role Role { get; set; } = null!;
}
