namespace Altima.Broker.Business.Types
{
    public class ImageType: BaseType<byte[]>
    {
        public ImageType(byte[] value) => Value = value;
        public static implicit operator ImageType(byte[] value) => new ImageType(value);
    }
}
