using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Telefone
{
    [TypeAttribute(Size = 10, Mask = "0999-9999")]
    public class Numero : StringType
    {
        public Numero(string value) : base(value)
        {
        }
    }
}
