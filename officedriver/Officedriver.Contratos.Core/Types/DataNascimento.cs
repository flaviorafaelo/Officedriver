using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    public struct DataNascimento : IDateType
    {
        private readonly string _value;
        private DataNascimento(string value) => _value = value;
        public static implicit operator DataNascimento(string value) => new DataNascimento(value);
    }
}
