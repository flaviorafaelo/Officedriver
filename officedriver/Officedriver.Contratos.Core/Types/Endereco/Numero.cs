using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 20)]
    public class Numero : StringType
    {

        //private Numero(string value) => Value = value;
        //public static implicit operator Numero(string value) => new Numero(value);
        public Numero(string value) : base(value)
        {
        }
    }
}
