namespace Core.UI.SearchContext.Component
{
    public enum ComponentType
    {
        Regular,
        Block,
        Popup
    }

    public struct ComponentMetadata(string name, ComponentType type)
    {
        public string Name { get; } = name;
        public ComponentType Type { get; } = type;
    }
}
