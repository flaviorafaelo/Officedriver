using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Telefone
{
    [TypeAttribute(Size = 2)]
    public struct Ddd : IStringType
    {
        private readonly string _value;
        private Ddd(string value) => _value = value;
        public static implicit operator Ddd(string value) => new Ddd(value);
    }
}
