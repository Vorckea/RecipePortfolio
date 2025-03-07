namespace RecipePortfolio.Utils
{
    /// <summary>
    /// Utility class for debouncing actions.
    /// </summary>
    public class DebounceUtils
    {
        /// <summary>
        /// Debounces an asynchronous action by delaying its execution until after a specified time interval has elapsed since the last time it was invoked.
        /// </summary>
        /// <param name="action">The asynchronous action to debounce.</param>
        /// <param name="cancellationTokenSource">A reference to the <see cref="CancellationTokenSource"/> used to cancel the previous action if it is still pending.</param>
        /// <param name="delay">The delay in milliseconds to wait before executing the action. Default is 300 milliseconds.</param>
        public static void Debounce(Func<Task> action, ref CancellationTokenSource? cancellationTokenSource, int delay = 300)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            Task.Delay(delay, token)
                .ContinueWith(async _ =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        await action();
                    }
                }, token);
        }
    }
}
