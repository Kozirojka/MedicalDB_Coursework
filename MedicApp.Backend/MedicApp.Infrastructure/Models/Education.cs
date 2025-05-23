﻿using System;
using System.Collections.Generic;

namespace MedicApp.Infrastructure.Models;

public partial class Education
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
