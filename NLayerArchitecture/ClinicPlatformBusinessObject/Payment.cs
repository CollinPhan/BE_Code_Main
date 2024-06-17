﻿using System;
using System.Collections.Generic;

namespace ClinicPlatformBusinessObject;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public bool Status { get; set; }

    public DateTime MadeOn { get; set; }

    public long Amount { get; set; }

    public Guid BookingId { get; set; }

    public int? PaymentTypeId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual PaymentType? PaymentType { get; set; }
}
