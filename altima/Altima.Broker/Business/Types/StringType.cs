namespace Altima.Broker.Business.Types
{
    public class StringType : BaseType<string>
    {
        public StringType(string value) => Value = value;
        public static implicit operator StringType(string value) => new StringType(value);
    }
}