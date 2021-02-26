using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Veiculo
{
    [TypeAttribute(Size = 7, Validation = @"/[A-Z]{3}[0-9][0-9A-Z][0-9]{2}/")]
    public class Placa : StringType
    {
        //private Placa(string value) => Value = value;
        //public static implicit operator Placa(string value) => new Placa(value);
        public Placa(string value) : base(value)
        {
        }
    }
}
