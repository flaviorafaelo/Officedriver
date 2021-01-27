using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Conta
{
    [TypeAttribute(Size = 5)]
    public struct Agencia : IStringType
    {
        private readonly string _value;
        private Agencia(string value) => _value = value;
        public static implicit operator Agencia(string value) => new Agencia(value);
        public override string ToString() => _value;
    }
}
