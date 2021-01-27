namespace Altima.Broker.Metadata
{
    public class Property: IProperty
    {
        public Property(string name, string type, string caption, string group, bool required)
        {
            Name = name;
            Type = type;
            Caption = caption;
            Group = group;
            Required = required;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Caption { get; set; }
        public string Group { get; set; }
        public bool Required { get; set; }
    }
}
