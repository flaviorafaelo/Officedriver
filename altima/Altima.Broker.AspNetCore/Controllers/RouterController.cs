using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Altima.Broker.System.Routes;
using System.Collections.Generic;

namespace Altima.Broker.AspNet.Mvc.Controllers
{
    [Route("api/routes")]
    public class RouterController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Module[]>> List()
        {
            IList<Action> actions = new List<Action>();

            actions.Add(new Action(1, "Criar", ActionType.Create, "DataEditComponent", null));
            actions.Add(new Action(1, "Alterar", ActionType.Update, "DataEditComponent",
                new Service("cooperado", new List<Param>()
                {
                    new Param("id","{data.id}")
                })));
            actions.Add(new Action(1, "Excluir", ActionType.Delete, "DataEditComponent",
                new Service("cooperado", new List<Param>()
                {
                    new Param("id","{data.id}")
                })));
            actions.Add(new Action(1, "Inativar", ActionType.None, "DataEditComponent",
                new Service("cooperado\\inativar", new List<Param>()
                {
                    new Param("id","{data.id}")
                })));

            IList<Route> routes = new List<Route>();

            //routes.Add(new Route("DASHBOARD", "Dashboard", "DashboardComponent", null, null));
            routes.Add(new Route("COOPERADO","Cadastros/Cooperados", "DataEditComponent", new Service("cooperado",null), actions));

            Module module = new Module("CONTRATO", "Contratos", routes);

            IList<Module> modules = new List<Module>();
            modules.Add(module);

            return Ok(modules);
        }
    }
}
