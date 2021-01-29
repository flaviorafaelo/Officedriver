using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct Moeda : IDecimalType
    {
        private readonly string _value;
        private Moeda(string value) => _value = value;
        public static implicit operator Moeda(string value) => new Moeda(value);
        public override string ToString() => _value;
    }
}
