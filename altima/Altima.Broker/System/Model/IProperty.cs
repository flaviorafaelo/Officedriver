using Altima.Broker.Business.Types;
using System;

namespace Altima.Broker.System
{
    public interface IProperty
    {
        DataType Type { get; }
        string TypeName { get; }
        string Name { get; }
        bool Required { get; }
        //Type PrimitiveType { get; }
        //   string ToString();
    }
}
