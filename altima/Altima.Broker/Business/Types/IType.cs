using System;

namespace Altima.Broker.Business.Types
{
    public interface IType<T>
    {
        T Value { get; set; }
        //Type PrimitiveType { get; }
    }
}
