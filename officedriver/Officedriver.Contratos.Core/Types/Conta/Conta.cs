using Altima.Broker.Business.Types;

namespace Officedriver.Contratos.Core.Types.Conta
{
    public class Conta : ObjectType
    {
        protected Conta(){}

        public Conta(string banco, Tipo tipo, Agencia agencia, Numero numero, char digito)
        {
         //   Banco = banco;
          //  Tipo = tipo;
            Agencia = agencia;
            Numero = numero;
          //  Digito = digito;
        }

       // public string Banco { get; set; }
        //public Tipo Tipo { get; set; }
        public Agencia Agencia { get; set; }
        public Numero Numero { get; set; }
       // public char Digito { get; set; }
    }
}
