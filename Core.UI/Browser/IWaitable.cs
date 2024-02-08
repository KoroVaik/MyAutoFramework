namespace Core.UI.Browser
{
    public interface IWaitable
    {
        bool WaitFor(Func<bool> func, int timeout = 4000, int tickSize = 200, string? exceptionText = null);

        bool WaitForOrContinue(Func<bool> func, int timeout = 4000, int tickSize = 200);

        bool WaitForAction(Action action, int timeout = 4000, int tickSize = 200, string? exceptionText = null);
    }
}