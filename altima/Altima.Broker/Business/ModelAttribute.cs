using System;

namespace Altima.Broker.Business
{
    public class ModelAttribute : Attribute
    {
        public bool Required { get; set; }
    }
}