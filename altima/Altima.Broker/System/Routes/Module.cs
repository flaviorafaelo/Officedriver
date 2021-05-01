using System.Collections.Generic;

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
    }
}
