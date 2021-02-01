using System.Collections.Generic;

namespace Altima.Broker.System.Serializable
{
    public class ItemFile
    {
        public string Property { get; set; }
        public string Label { get; set; }
        public string VisibilityRule { get; set; }
        public string EnablingRule { get; set; }
        public string Tooltip { get; set; }
    }

    public class GroupFile
    {
        public string Description { get; set; }
        public IList<ItemFile> Items { get; set; }
    }

    public class PageFile
    {
        public string Description { get; set; }
        public IList<GroupFile> Groups { get; set; }
    }


    public class ViewFile
    {

        public string Description { get;  set; }
        public int Version { get; set; }
        private string Object { get; set; }
        public IList<PageFile> Pages { get; set; }
    }
}
