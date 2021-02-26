using System;

namespace Altima.Broker.Business.Types
{
    public class BaseType<T> : IType<T>
    {
        protected BaseType() { }
        protected BaseType(T value) => Value = value;
        public T Value { get; set; }

        // public Type PrimitiveType { get => typeof(T); }
        //public override string ToString() => (string)Value;
    }
}
