using System.Collections.Generic;

namespace Altima.Broker.System.Routes
{
    public  class Route
    {
        public Route(string id, string display, string target, Service service, IList<Action> actions)
        {
            Id = id;
            Display = display;
            Target = target;
            Service = service;
            Actions = actions;
        }

        public string Id { get; }
        public string Display { get; }
        public string Target { get; }
        public Service Service { get; }
        public IList<Action> Actions { get; }
    }
}
