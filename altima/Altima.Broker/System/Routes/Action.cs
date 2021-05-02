namespace Altima.Broker.System.Routes
{
    public enum ActionType
    { 
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        List = 4,
        CRUD = 5
    }

    public class Action
    {
        public Action(string id, string display, ActionType type, string target, Service service, string url)
        {
            Id = id;
            Display = display;
            Type = type;
            Target = target;
            Service = service;
            Url = url;
        }

        public string Id { get; set; }
        public string Display { get; }
        public string Url { get; set; }
        public ActionType Type { get; }
        public string Target { get; }
        public Service Service { get; }

    }
}