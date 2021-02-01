using System.Collections.Generic;

namespace Altima.Broker.System
{
    public class View
    {
        public View(string description, int version, Model @object, IList<Page> pages)
        {
            Description = description;
            Version = version;
            Object = @object;
            Pages = pages;
        }

        public string Description { get; private set; }
        public int Version { get; private set; }
        private Model Object { get; set; }
        public IList<Page> Pages { get; private set; }
    }
}
