namespace E050_CEF;

internal static class AsyncContext
{
    public static void Run(Func<Task> func)
    {
        var prevCtx = SynchronizationContext.Current;

        try
        {
            var syncCtx = new SingleThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncCtx);

            Task t = func();
            t.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);
            syncCtx.RunOnCurrentThread();
            t.GetAwaiter().GetResult();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(prevCtx);
        }
    }
}