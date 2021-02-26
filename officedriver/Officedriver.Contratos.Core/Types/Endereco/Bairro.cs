using Altima.Broker.Business;
using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Endereco
{
    [TypeAttribute(Size = 50)]
    public class Bairro : StringType
    {
        //private Bairro(string value) => Value = value;
        //public static implicit operator Bairro(string value) => new Bairro(value);
        public Bairro(string value) : base(value)
        {
        }
    }
}
