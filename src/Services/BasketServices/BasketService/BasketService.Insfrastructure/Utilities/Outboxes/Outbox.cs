using BasketService.Domain.SeedWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BasketService.Insfrastructure.Utilities.Outboxes
{
    public class Outbox : IEntity
    {
        [JsonConstructor]
        public Outbox()
        {
        }
        [JsonProperty]
        public Guid Id { get; private set; }
        [JsonProperty]
        public Guid EventId { get; private set; }
        [JsonProperty]
        public string? IntegrationEventName { get; private set; }
        [JsonProperty]
        public string? IntegrationEventType { get; private set; }
        [JsonProperty]
        public string? Content { get; private set; } = "{}";
        public OutboxState State { get; private set; }
        private readonly Dictionary<string, object> DomainEventDictionary = [];
        public void InitOutbox(IOutboxMessage integrationEvent)
        {
            Id = Guid.NewGuid();
            EventId = integrationEvent.EventId;
            State = integrationEvent.State;
            IntegrationEventName = integrationEvent.GetType().Name;
            IntegrationEventType = integrationEvent.GetType().ToString();
            Content = JsonConvert.SerializeObject(integrationEvent);
        }
        public void AddDomainEventDictionary(string entityType, object entity)
        {
            DomainEventDictionary.Add(entityType, entity);
        }
        public void DomainEventDictionaryToContent()
        {
            var doc1 = JObject.Parse(Content!);
            doc1.Merge(JObject.Parse(JsonConvert.SerializeObject(DomainEventDictionary)), new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
            Content = doc1.ToString();
        }
    }
}
