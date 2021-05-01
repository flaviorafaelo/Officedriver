using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using routes = Altima.Broker.System.Routes;
using System.Collections.Generic;
using Altima.Broker.Core;
using System;
using System.Text;

namespace Altima.Broker.AspNet.Mvc.Controllers
{
    [Route("api/routes")]
    public class RouterController : ControllerBase
    {
        private IApplicationBroker _applcationBroker;
        private IList<routes.Module> _modules;  

        public RouterController(IApplicationBroker applcationBroker)
        {
            _applcationBroker = applcationBroker;
            _modules = _applcationBroker.Modules;
        }

        [HttpGet]
        public async Task<ActionResult<routes.Module[]>> List()
        {
            return Ok(_modules);
        }

        [HttpGet]
        [Route("horas")]
        public async Task<ActionResult<string>> TotalHoras()
        {
         
            TimeSpan InicioHoraDiurna = new TimeSpan(6, 0, 0);
            TimeSpan FinalHoraDiurna = new TimeSpan(22, 0, 0);
            TimeSpan totalhorasD = new TimeSpan(0, 0, 0);
            TimeSpan totalhorasN = new TimeSpan(0, 0, 0);
            DateTime entrada = new DateTime(2021, 05, 01, 11, 0, 0);
            DateTime saida = new DateTime(2021, 05, 02, 23, 30, 0);
            string totalD = "";
            string totalN = "";
            //HD
            if (entrada.TimeOfDay < InicioHoraDiurna)
                totalhorasD = +InicioHoraDiurna - entrada.TimeOfDay;
            if (saida.TimeOfDay > FinalHoraDiurna)
                totalhorasD = +saida.TimeOfDay - FinalHoraDiurna;
            totalD = ((saida - entrada) - totalhorasD).ToString(@"hh\:mm");

            //HN
            if (entrada.TimeOfDay < InicioHoraDiurna)
                totalhorasN = +InicioHoraDiurna - entrada.TimeOfDay;
            if (saida.TimeOfDay > FinalHoraDiurna)
                totalhorasN = +saida.TimeOfDay - FinalHoraDiurna;
            totalN = totalhorasN.ToString(@"hh\:mm");




            return Ok(totalD + " - " + totalN);
        }

        [HttpGet]
        [Route("{routeName}/actions")]
        public async Task<ActionResult<Action[]>> ListActions(string routeName)
        {
            IList<routes.Action> actions = new List<routes.Action>();
            foreach (var module in _modules)                    
            {
                actions = module.GetActionsByRoute(Encoding.UTF8.GetString(Convert.FromBase64String(routeName)));
                if (actions != null)
                    break;
            }
            return Ok(actions);
        }

    }
}
