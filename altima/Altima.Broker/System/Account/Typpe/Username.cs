using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Altima.Broker.System.Type
{
    [TypeAttribute(Size = 256)]
    public class Username : StringType
    {
        public Username(string value) : base(value)
        {
        }
    }
} 