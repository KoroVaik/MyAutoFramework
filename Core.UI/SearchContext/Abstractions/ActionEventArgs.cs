namespace Core.UI.SearchContext.Abstractions
{
    public class ContextEventArgs : EventArgs
    {
        public Exception? Exception { get; private set; }
        public ContextEventArgs(Exception? exception = null)
        {
            Exception = exception;
        }
    }

    public class ActionEventArgs : EventArgs
    {
        public string ActionName { get; private set; }
        public object[] ActionArguments { get; private set; }
        public object? ReturningValue { get; private set; }
        public Exception? Exception { get; private set; }
        public ActionEventArgs(string actionName, object[] actionArguments, object? returningValue, Exception? exception = null)
        {
            ActionName = actionName;
            ActionArguments = actionArguments;
            ReturningValue = returningValue;
            Exception = exception;
        }
    }
}
