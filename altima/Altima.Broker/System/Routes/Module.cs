using System.Collections.Generic;
using System.Linq;

namespace Altima.Broker.System.Routes
{
    public class Module
    {
        public Module(string id, string name, IList<Route> routes)
        {
            Id = id;
            Name = name;
            Routes = routes;
        }

        public string Id { get; }
        public string Name { get; }
        public IList<Route> Routes { get; }

        public IList<Action> GetActionsByRoute(string routeId)
        {
            return Routes.Where(a => a.Display == routeId).FirstOrDefault().Actions;
        }
    }
}
