namespace Altima.Broker.System
{
    public class Item
    {
        public Item(IProperty property, string label, string visibilityRule, string enablingRule, string tooltip)
        {
            Property = property;
            Label = label;
            VisibilityRule = visibilityRule;
            EnablingRule = enablingRule;
            Tooltip = tooltip;
        }

        public IProperty Property { get; set; }
        public string Label { get; set; }
        public string VisibilityRule { get; set; }
        public string EnablingRule { get; set; }
        public string Tooltip { get; set; }
    }
}
