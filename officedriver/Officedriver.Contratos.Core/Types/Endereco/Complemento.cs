using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 50)]
    public class Complemento : StringType
    {
        public Complemento(string value) : base(value)
        {
        }
    }
}
