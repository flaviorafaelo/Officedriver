using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types
{
    [TypeAttribute(Size = 10)]
    public class Matricula : StringType
    {
        public Matricula(string value) : base(value)
        {
        }
    }
}