using Altima.Broker.Business;
using Officedriver.Contratos.Core.Contratos.Model;
using Officedriver.Contratos.Core.Cooperados.Model;
using Officedriver.Contratos.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Officedriver.Contratos.Core.Horas
{
    public class Apontamento: BaseModel
    {
        //[ModelAttribute(Required = true)]
        //public Contrato Contrato { get; set; }
        //[ModelAttribute(Required = true)]
        //public Cooperado Cooperado { get; set; }
        //[ModelAttribute(Required = true)]
        //public Veiculo Veiculo { get; set; }
        //[ModelAttribute(Required = true)]
        public DataHora Entrada { get; set; }
        [ModelAttribute(Required = true)]
        public DataHora Saida { get; set; }

        public Kilometragem KMInicial { get; set; }
        public Kilometragem KMFinal { get; set; }
        public Kilometragem Intervalo { get; set; }
        public bool Viagem { get; set; }
   //     public IEnumerable<Reembolso> Reembolsos { get; set; }
    }
}
