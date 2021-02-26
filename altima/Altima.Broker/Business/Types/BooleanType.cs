namespace Altima.Broker.Business.Types
{
    public class BooleanType : BaseType<bool>
    {
        public BooleanType(bool value) => Value = value;
        public static implicit operator BooleanType(bool value) => new BooleanType(value);
    }
}
