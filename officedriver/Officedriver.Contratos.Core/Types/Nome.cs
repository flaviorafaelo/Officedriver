using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 200)]
    public struct Nome : IStringType
    {
        private readonly string _value;        
        private Nome(string value) => _value = value;
        public static implicit operator Nome(string value) => new Nome(value);
        public override string ToString() => _value;
    }
}
