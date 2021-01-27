namespace Altima.Broker.Metadata
{
    public interface IProperty
    {
        string Name { get; set; }
        string Type { get; set; }
        string Caption { get; set; }
        bool Required { get; set; }
        string Group { get; set; }
    }
}
