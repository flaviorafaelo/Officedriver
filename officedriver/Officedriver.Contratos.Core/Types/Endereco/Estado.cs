using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 50)]
    public class Estado : StringType
    {
        // private Estado(string value) => Value = value;
        // public static implicit operator Estado(string value) => new Estado(value);
        public Estado(string value) : base(value)
        {
        }
    }
}
