using Core.Logger.Allure;
using NUnit.Framework;

namespace Core
{
    public static class TearDownActions
    {
        private static readonly ThreadLocal<Stack<TearDownActionItem>> _actions = new(() => new());
        public static ThreadLocal<bool> IsAbleToStopWithWarning = new(() => true);

        public static TearDownActionItem Add(Action actionToAdd)
        {
            Log.WriteConsoleAllureLine($"Add to 'TearDownActionItem' method: {actionToAdd.Method.Name}");
            TearDownActionItem actionItem = new(actionToAdd, false);
            _actions.Value!.Push(actionItem);
            return actionItem;
        }

        public static void Invoke()
        {
            Log.WriteConsoleAllureLine($"Start invoke 'TearDownActions' for: {TestContext.CurrentContext.Test.Name}");
            IsAbleToStopWithWarning.Value = false;
            var exceptions = new List<Exception>();
            while (_actions.Value!.Count > 0)
            {
                var actionItem = _actions!.Value.Pop();
                try
                {
                    if (!actionItem.IsIgnored) actionItem.Invoke();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            CleanUp();

            if (exceptions.Any()) throw new Exception($"Following exceptions were thrown while TearDown:{Environment.NewLine}"
                + string.Join(Environment.NewLine, exceptions.Select(e => e.ToString())));
        }

        public static void StopTestWithWarning(string warningMessage)
        {
            if (IsAbleToStopWithWarning.Value)
            {
                Add(() => Assert.Warn(warningMessage));
            }
            Assert.Pass(warningMessage);
        }

        private static void CleanUp()
        {
            _actions.Value!.Clear();
            IsAbleToStopWithWarning.Value = true;
        }
    }

    public class TearDownActionItem
    {
        private readonly Action _action;

        public bool IsIgnored { get; private set; }

        public TearDownActionItem(Action action, bool isIgnored)
        {
            _action = action;
            IsIgnored = isIgnored;
        }

        public void SkipExecution()
        {
            IsIgnored = true;
        }

        public void Invoke()
        {
            _action.Invoke();
        }
    }
}
