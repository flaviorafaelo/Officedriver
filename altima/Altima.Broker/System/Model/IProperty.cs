using Altima.Broker.Business.Types;

namespace Altima.Broker.System
{
    public interface IProperty
    {
        DataType Type { get; }
        string TypeName { get; }
        string Name { get; }
        bool Required { get; }
     //   string ToString();
    }
}
