using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Contrato
{
    [TypeAttribute(Size = 20)]
    public struct Numero : IStringType
    {
        private readonly string _value;
        private Numero(string value) => _value = value;
        public static implicit operator Numero(string value) => new Numero(value);
    }
}
