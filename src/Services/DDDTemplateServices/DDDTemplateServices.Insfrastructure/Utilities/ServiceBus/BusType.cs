using MassTransit;
namespace DDDTemplateServices.Insfrastructure.Utilities.ServiceBus
{
    /// <summary>
    /// Message Types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusType<T> : MessageUrnAttribute
      where T : IMessage
    {
        public BusType()
            : base("scheme:" + typeof(T).Name.Underscore(), false)
        {
        }
    }
}