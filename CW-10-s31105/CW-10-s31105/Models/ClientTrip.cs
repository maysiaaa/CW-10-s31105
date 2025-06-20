﻿using System;
using System.Collections.Generic;

namespace CW_10_s31105.Models;

public partial class ClientTrip
{
    public int IdClient { get; set; }

    public int IdTrip { get; set; }

    public int RegisteredAt { get; set; }

    public int? PaymentDate { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Trip IdTripNavigation { get; set; } = null!;
}
