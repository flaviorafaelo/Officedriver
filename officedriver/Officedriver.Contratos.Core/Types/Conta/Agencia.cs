using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Conta
{
    [TypeAttribute(Size = 10)]
    public class Agencia : StringType
    {
        public Agencia(string value) : base(value)
        {
        }
    }
}
