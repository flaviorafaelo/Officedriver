using System;

namespace Altima.Broker.Business
{
    public class TypeAttribute : Attribute
    {
        public int Size { get; set; }
        public int MaxLen { get; set; }
        public int MinLen { get; set; }
        public string Mask { get; set; }
        public string Validation { get; set; }
    }
}
