namespace RecipePortfolio.Utils
{
    public class DebounceUtils
    {
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
