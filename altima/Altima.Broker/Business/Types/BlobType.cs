namespace Altima.Broker.Business.Types
{
    public class BlobType : BaseType<string>
    {
        public BlobType(string value) => Value = value;
        public static implicit operator BlobType(string value) => new BlobType(value);
    }
}
