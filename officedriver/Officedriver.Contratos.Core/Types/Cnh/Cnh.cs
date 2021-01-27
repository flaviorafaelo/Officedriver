using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Cnh
{
    public struct Cnh : IObjectType
    {
        public Cnh(Numero numero, Validade validade, Categoria categoria)
        {
            Numero = numero;
            Validade = validade;
            Categoria = categoria;
        }
        public Numero Numero { get; set; }
        public Validade Validade { get; set; }
        public Categoria Categoria { get; set; }
    }
}
