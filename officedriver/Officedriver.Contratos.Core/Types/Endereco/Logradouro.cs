using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 200)]
    public class Logradouro : StringType
    {
        public Logradouro(string value) : base(value)
        {
        }
    }
}
