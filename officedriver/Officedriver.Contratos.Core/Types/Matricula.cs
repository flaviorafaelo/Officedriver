using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 10)]
    public struct Matricula : IStringType
    {
        private readonly string _value;
        private Matricula(string value) => _value = value;
        public static implicit operator Matricula(string value) => new Matricula(value);
        public override string ToString() => _value;
    }
}