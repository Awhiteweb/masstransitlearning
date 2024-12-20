﻿using MassTransit;

namespace Oval.MassTransitLearning.API.Events
{
    public class OrderPlaced : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }

        public string[] Items { get; init; } = [];
    }
}
