using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Cnh
{
    public class Cnh : ObjectType
    {
        protected Cnh() { }

        public Cnh(Numero numero, Data validade, Categoria categoria)
        {
            Numero = numero;
            Validade = validade;
            // Categoria = categoria;
        }
        public Numero Numero { get; set; }
        public Data Validade { get; set; }
       // public Categoria Categoria { get; set; }
    }
}
