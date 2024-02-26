using MassTransit;
namespace STemplate.Insfrastructure.Utilities.ServiceBus
{
    /// <summary>
    /// Message Types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UrnType<T> : MessageUrnAttribute
       where T : IMessage
    {
        public UrnType()
            : base("scheme:" + typeof(T).Name.Underscore(), false)
        {
        }
    }
}