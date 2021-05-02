using System.Collections.Generic;

namespace Altima.Broker.System.Routes
{
    public enum RulesType
    {
        Admin = 0,
        Cooperative = 1,
        Costumer = 2,
        All = 3
    }
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

        public string Id { get; set; }
        public string Display { get; }
        public string Target { get; }
        public string Url { get; set; }
        public Service Service { get; }
        public RulesType Rule { get; }
        public IList<Action> Actions { get; }
    }
}
