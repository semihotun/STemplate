using System.ComponentModel;

namespace PaymentService.Insfrastructure.Utilities.Grid.Filter
{
    /// <summary>
    /// filter operator for dynamic grid
    /// </summary>
    public enum FilterOperators
    {
        [Description("Eşittir")]
        Equals = 1,
        [Description("Eşit Değil")]
        NotEquals = 2,
        [Description("İçerir")]
        Contains = 3,
        [Description("x Sayısından büyük")]
        GreaterThan = 5,
        [Description("x sayısından küçük")]
        LessThan = 6,
    }
}
