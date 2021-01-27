using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 200)]
    public struct Logradouro : IStringType
    {
        private readonly string _value;
        private Logradouro(string value) => _value = value;
        public static implicit operator Logradouro(string value) => new Logradouro(value);
    }
}
