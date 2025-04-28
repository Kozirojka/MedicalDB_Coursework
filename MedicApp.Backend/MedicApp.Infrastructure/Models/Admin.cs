using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Admin
{
    public int Id { get; set; }

    public int Accountid { get; set; }

    public virtual Account Account { get; set; } = null!;
}
