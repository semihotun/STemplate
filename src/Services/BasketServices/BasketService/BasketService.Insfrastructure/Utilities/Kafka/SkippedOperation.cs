namespace BasketService.Insfrastructure.Utilities.Kafka
{
    public sealed class SkippedOperation
    {
        public static readonly SkippedOperation Create = new("c");
        public static readonly SkippedOperation Update = new("u");
        public static readonly SkippedOperation Delete = new("d");
        public static readonly SkippedOperation CreateAndUpdate = new("cu");
        public static readonly SkippedOperation CreateAndDelete = new("cd");
        public static readonly SkippedOperation UpdateAndDelete = new("ud");
        public static readonly SkippedOperation Empty = new("");
        private SkippedOperation(string value)
        {
            this.Value = value;
        }
        public string Value { get; }
    }
}
