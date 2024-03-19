using OrderService.Insfrastructure.Utilities.Outboxes;
using System.Text.Json.Serialization;

namespace OrderService.Insfrastructure.Utilities.Kafka
{
    public class KafkaOutboxModel
    {
        public class KafkaParameters(string allowed)
        {
            public string Allowed { get; set; } = allowed;
        }
        public class KafkaPayload
        {
            public object? Before { get; set; }
            public Outbox? After { get; set; }
            public KafkaSource? Source { get; set; }
            public string? Pp { get; set; }
            public long? Ts_ms { get; set; }
            public object? Transaction { get; set; }
        }
        public class KafkaSchema(string type,
            List<KafkaField> fields,
            bool optional,
            string name,
            int version)
        {
            public string Type { get; set; } = type;
            public List<KafkaField> Fields { get; set; } = fields;
            public bool Optional { get; set; } = optional;
            public string Name { get; set; } = name;
            public int Version { get; set; } = version;
        }
        public class KafkaField(string type,
        List<KafkaField> fields,
        bool optional,
        string name,
        string field,
        int? version,
        KafkaParameters parameters,
        string @default)
        {
            public string Type { get; set; } = type;
            public List<KafkaField> Fields { get; set; } = fields;
            public bool Optional { get; set; } = optional;
            public string Name { get; set; } = name;
            public string Field { get; set; } = field;
            public int? Version { get; set; } = version;
            public KafkaParameters Parameters { get; set; } = parameters;
            public string Default { get; set; } = @default;
        }
        public class KafkaSource(string version,
        string connector,
        string name,
        long ts_ms,
        string snapshot,
        string db,
        object sequence,
        string schema,
        string table,
        string change_lsn,
        string commit_lsn,
        int event_serial_no)
        {
            public string Version { get; set; } = version;
            public string Connector { get; set; } = connector;
            public string Name { get; set; } = name;
            public long Ts_ms { get; set; } = ts_ms;
            public string Snapshot { get; set; } = snapshot;
            public string Db { get; set; } = db;
            public object Sequence { get; set; } = sequence;
            public string Schema { get; set; } = schema;
            public string Table { get; set; } = table;
            public string Change_lsn { get; set; } = change_lsn;
            public string Commit_lsn { get; set; } = commit_lsn;
            public int Event_serial_no { get; set; } = event_serial_no;
        }
        [JsonConstructor]
        public KafkaOutboxModel(KafkaSchema schema,
            KafkaPayload payload)
        {
            Schema = schema;
            Payload = payload;
        }
        public KafkaSchema Schema { get; set; }
        public KafkaPayload Payload { get; set; }
    }
}
