using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 9, MinLen = 9, MaxLen = 9, Validation = @"/^([\d]{2})\.?([\d]{3})\-?([\d]{3})/")]
    public class Cep : IStringType
    {
        private readonly string _value;
        private Cep(string value) => _value = value;
        public static implicit operator Cep(string value) => new Cep(value);
    }
}
