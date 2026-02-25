using Core.Logger.Allure;
using Polly;
using Polly.Timeout;
using System.Diagnostics;

namespace Core
{
    public class WaitHelper
    {
        bool _loggingEnabled;
        TimeoutStrategy _timeoutStrategy;

        public static WaitHelper Wait => new WaitHelper();
        public const int LongWait = 60_000;
        //static IEnumerable<TimeSpan> GetDelays() => Backoff.LinearBackoff(TimeSpan.FromMilliseconds(100), retryCount: 5, factor: 1.5, fastFirst: true);

        public WaitHelper(bool loggingEnabled = true, TimeoutStrategy timeoutStrategy = TimeoutStrategy.Pessimistic)
        {
            _loggingEnabled = loggingEnabled;
            _timeoutStrategy = timeoutStrategy;
        }

        public bool Until(Func<bool> condition, string exceptionText, int timeout = 8000, int bitweenAttemptDelay = 300, bool throwOnFailure = true)
        {
            if (_loggingEnabled)
            {
                var callerName = new StackTrace().GetFrame(2)!.GetMethod()!.Name;
                //Log.WriteConsoleAllureLine($"Start 'Wait Until' in method: {callerName}");
                _loggingEnabled = false;
            }
            return ForNotDefault(condition, exceptionText, timeout, bitweenAttemptDelay, throwOnFailure);
        }

        public T ForNotDefault<T>(Func<T> func, string exceptionText, int timeout = 8000, int bitweenAttemptsDelay = 300, bool throwOnFailure = true)
        {
            if (_loggingEnabled)
            {
                var callerName = new StackTrace().GetFrame(2)!.GetMethod()!.Name;
                //Log.WriteConsoleAllureLine($"Start 'Wait For Not Default' in method: {callerName}");
                _loggingEnabled = false;
            }
            return ForCondition(
                func,
                (value) => !EqualityComparer<T>.Default.Equals(value, default),
                exceptionText,
                timeout,
                bitweenAttemptsDelay,
                throwOnFailure);
        }

        public T ForCondition<T>(Func<T> func, Func<T, bool> acceptableValueCondition, string exceptionText, int timeout = 8000, int bitweenAttemptDelay = 300, bool throwOnFailure = true)
        {
            if (_loggingEnabled)
            {
                var callerName = new StackTrace().GetFrame(2)!.GetMethod()!.Name;
                //Log.WriteConsoleAllureLine($"Start 'Wait For Condition' in method: {callerName}");
            }
            List<Exception> exceptions = new();
            var wait = Policy.Timeout(TimeSpan.FromMilliseconds(timeout), _timeoutStrategy).Wrap(
                Policy.Handle<Exception>().OrResult<T>(handledResult => !acceptableValueCondition(handledResult))
                        .WaitAndRetryForever(
                            sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(bitweenAttemptDelay/* * retryAttempt*/),
                            (res, _) => exceptions.Add(res.Exception)
                        ));
            var result = wait.ExecuteAndCapture(() => func());

            if (throwOnFailure && result.Outcome == OutcomeType.Failure)
            {
                if (exceptions.Any())
                {
                    var finalException = new TimeoutException(
                    $"Timed out after {exceptions.Count} attemts in total {timeout}ms ({bitweenAttemptDelay}ms delay between attempts).{Environment.NewLine}" +
                    $"Error message: {exceptionText}");
                    if (exceptions.Last() is Exception)
                        finalException.Data.Add("Last exception: ", exceptions.Last());
                    else
                        finalException.Data.Add("Last wrong value: ", result.FinalHandledResult);

                    throw finalException;
                }
                else
                {
                    throw new TimeoutException($"Func didn't finish in total {timeout}ms" +
                        $"Error message: {exceptionText}");
                }
            }
            return result.Result;
        }

        public void ForAction(Action action, string exceptionText, int timeout = 20000, int bitweenAttemptDelay = 300, bool throwOnFailure = true)
        {
            if (_loggingEnabled)
            {
                var callerName = new StackTrace().GetFrame(2)!.GetMethod()!.Name;
                //Log.WriteConsoleAllureLine($"Start 'Wait For Action' in method: {callerName}");
            }
            List<Exception> exceptions = new();
            var wait = Policy.Timeout(TimeSpan.FromMilliseconds(timeout), _timeoutStrategy).Wrap(
                Policy.Handle<Exception>()
                        .WaitAndRetryForever(
                            sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(bitweenAttemptDelay/* * retryAttempt*/),
                            (excpt, _) => exceptions.Add(excpt)
                        ));
            var result = wait.ExecuteAndCapture(() => action());

            if (throwOnFailure && result.Outcome == OutcomeType.Failure)
            {
                if (exceptions.Any())
                {
                    var finalException = new TimeoutException(
                        $"Timed out after {exceptions.Count} attemts in total {timeout}ms ({bitweenAttemptDelay}ms delay between attempts).{Environment.NewLine}" +
                        $"Error message: {exceptionText}");
                    finalException.Data.Add("Last exception: ", exceptions.Last());
                    throw finalException;
                }
                else
                {
                    throw new TimeoutException($"Action didn't finished in total {timeout}ms{Environment.NewLine}" +
                        $"Error message: {exceptionText}");
                }
            }
        }
    }
}

