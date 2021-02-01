using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class Group
    {
        public Group(string description, IList<Item> items)
        {
            Description = description;
            Items = items;
        }

        public string Description { get; private set; }
        public IList<Item> Items { get; private set; }
    }
}
