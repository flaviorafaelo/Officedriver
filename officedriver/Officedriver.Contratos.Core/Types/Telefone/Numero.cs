using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Telefone
{
    [TypeAttribute(Size = 10, Mask = "0999-9999")]
    public struct Numero : IStringType
    {
        private readonly string _value;
        private Numero(string value) => _value = value;
        public static implicit operator Numero(string value) => new Numero(value);
    }
}
