using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct Inteiro : IIntegerType
    {
        private readonly string _value;
        private Inteiro(string value) => _value = value;
        public static implicit operator Inteiro(string value) => new Inteiro(value);
    }
}
