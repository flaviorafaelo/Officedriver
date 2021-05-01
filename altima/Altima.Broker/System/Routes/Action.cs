namespace Altima.Broker.System.Routes
{
    public enum ActionType
    { 
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        List = 4
    }

    public class Action
    {
        public Action(int id, string display, ActionType type, string target, Service service)
        {
            Id = id;
            Display = display;
            Type = type;
            Target = target;
            Service = service;
        }

        public int Id { get;  }
        public string Display { get; }
        public ActionType Type { get; }
        public string Target { get; }
        public Service Service { get; }
    }
}