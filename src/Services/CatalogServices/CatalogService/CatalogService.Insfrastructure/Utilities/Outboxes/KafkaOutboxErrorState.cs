﻿namespace CatalogService.Insfrastructure.Utilities.Outboxes
{
    public enum KafkaOutboxErrorState
    {
        NoError = 1,
        KafkaError = 2,
    }
}
