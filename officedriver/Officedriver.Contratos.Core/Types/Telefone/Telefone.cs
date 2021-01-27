using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Telefone
{
    public struct Telefone: IObjectType
    {
        public Telefone(Ddd ddd, Numero numero)
        {
            Ddd = ddd;
            Numero = numero;
        }
        public Ddd Ddd { get; set; }
        public Numero Numero { get; set; }
    }
}
