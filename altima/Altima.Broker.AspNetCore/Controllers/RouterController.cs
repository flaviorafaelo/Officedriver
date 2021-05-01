using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Altima.Broker.System.Routes;
using System.Collections.Generic;
using Altima.Broker.Core;

namespace Altima.Broker.AspNet.Mvc.Controllers
{
    [Route("api/routes")]
    public class RouterController : ControllerBase
    {
        private IApplicationBroker _applcationBroker;
        private IList<Module> _modules;  

        public RouterController(IApplicationBroker applcationBroker)
        {
            _applcationBroker = applcationBroker;
            _modules = _applcationBroker.Modules;
        }

        [HttpGet]
        public async Task<ActionResult<Module[]>> List()
        {
            return Ok(_modules);
        }

        [HttpGet]
        [Route("{routeName}/actions")]
        public async Task<ActionResult<Action[]>> ListActions(string routeName)
        {
            IList<Action> actions = new List<Action>();
            foreach (var module in _modules)
            {
                actions = module.GetActionsByRoute(routeName);
                if (actions != null)
                    break;
            }
            return Ok(actions);
        }

    }
}
