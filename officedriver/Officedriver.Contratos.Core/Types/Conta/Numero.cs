using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Conta
{
    [TypeAttribute(Size = 20)]
    public class Numero : StringType
    {

        public Numero(string value) : base(value)
        {
        }
    }
}
