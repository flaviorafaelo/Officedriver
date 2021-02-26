using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 200)]
    public class Nome : StringType
    {
        public Nome(string value) : base(value)
        {
        }
    }
}
