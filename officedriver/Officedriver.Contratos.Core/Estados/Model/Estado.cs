using Altima.Broker.Business;
using Officedriver.Contratos.Core.Types;

namespace Officedriver.Contratos.Core.Estados.Model
{
    public class Estado: BaseModel
    {
        public string Sigla { get; set; }
        public Nome Nome { get; set; }
    }
}
