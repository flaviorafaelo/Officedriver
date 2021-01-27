using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct Valor : IDecimalType
    {
        private readonly string _value;
        private Valor(string value) => _value = value;
        public static implicit operator Valor(string value) => new Valor(value);
        public override string ToString() => _value;
    }
}
