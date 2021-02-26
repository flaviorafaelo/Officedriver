namespace Altima.Broker.Business.Types
{
    public class DecimalType : BaseType<decimal>
    {
        //byte Precision { get; set; }
        public DecimalType(decimal value) => Value = value;
        public static implicit operator DecimalType(decimal value) => new DecimalType(value);
    }
}
