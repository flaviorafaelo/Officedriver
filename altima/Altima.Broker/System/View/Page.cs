using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class Page
    {
        public Page(string description, IList<Group> groups)
        {
            Description = description;
            Groups = groups;
        }

        public string Description { get; private set; }
        public IList<Group> Groups { get; private set; }
    }
}
