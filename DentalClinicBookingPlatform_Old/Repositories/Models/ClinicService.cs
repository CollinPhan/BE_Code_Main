﻿using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class ClinicService
{
    public Guid ClinicServiceId { get; set; }

    public long? Price { get; set; }

    public string? Description { get; set; }

    public int ClinicId { get; set; }

    public int ServiceId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Clinic Clinic { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
