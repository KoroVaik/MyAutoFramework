namespace Core.UI.BaseComponents.Interfaces
{
    public interface IForm : ISetValue, IGetValue
    {
        void Submit();
    }

    public interface ISetValue
    {
        void SetValue(string value);
    }

    public interface IGetValue
    {
        string GetValue();
    }
}
