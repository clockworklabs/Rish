namespace RishUI
{
    public interface IManualStyling
    {
        void OnName(string name);
        void OnClasses(ClassName className);
        void OnInline(Style style);
    }
}