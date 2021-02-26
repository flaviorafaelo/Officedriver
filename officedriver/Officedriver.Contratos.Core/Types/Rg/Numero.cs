using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Rg
{
    [TypeAttribute(Size = 30)]
    public class Numero : StringType
    {
        public Numero(string value) : base(value)
        {
        }
    }
}
