using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Telefone
{
    [TypeAttribute(Size = 3)]
    public class Ddd : StringType
    {
        public Ddd(string value) : base(value)
        {
        }
    }
}
