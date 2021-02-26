namespace Altima.Broker.Business.Types
{
    public class IntegerType : BaseType<int>
    {
        public IntegerType(int value) => Value = value;
        public static implicit operator IntegerType(int value) => new IntegerType(value);
    }
}
