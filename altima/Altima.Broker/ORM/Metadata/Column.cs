namespace Altima.Broker.Relational.Metadata
{
    public class Column
    {
        public Column(string name, string type, int size, int precision, int scale, bool notNull)
        {
            Name = name;
            Type = type;
            Size = size;
            Precision = precision;
            Scale = scale;
            NotNull = notNull;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool NotNull { get; set; }
    }
}
