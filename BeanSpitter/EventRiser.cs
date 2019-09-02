namespace BeanSpitter
{
    using BeanSpitter.Interfaces;
    using System;
#if DEBUG
    using System.Diagnostics;
#endif
    using System.Threading;
    using System.Threading.Tasks;

    public class EventRiser<T> : IEventRiserClass<T> where T : EventArgs
    {
        private readonly TaskScheduler taskScheduler;
        private long activeTasks;

        public EventRiser()
        {
            // https://devblogs.microsoft.com/premier-developer/limiting-concurrency-for-faster-and-more-responsive-apps/
            taskScheduler =
                new ConcurrentExclusiveSchedulerPair(
                    TaskScheduler.Default,          // schedule work to the ThreadPool
                    Environment.ProcessorCount * 2) // Schedule enough to keep all threads busy, with a queue to quickly replace completed work
                .ConcurrentScheduler;
        }


        public void RaiseEventsOnThreadPool(Func<object, T, CancellationToken, Task> evt, T args, CancellationToken cancellationToken)
        {
            var handler = evt;

            if (handler == null)
            {
                return;
            }

            var listeners = handler.GetInvocationList();

            if (listeners == null)
            {
                return;
            }

            if (listeners.Length == 0)
            {
                return;
            }

            foreach (var listener in listeners)
            {
                if (cancellationToken.IsCancellationRequested)
                {
#if DEBUG
                    Debug.WriteLine("Operation Cancelled - Cancel Event Raising");
#endif
                    break;
                }

                var method = (Func<object, T, CancellationToken, Task>)listener;
                // https://devblogs.microsoft.com/premier-developer/limiting-concurrency-for-faster-and-more-responsive-apps/
                Task.Factory.StartNew(async () =>
                {
                    Interlocked.Increment(ref activeTasks);
                    await method(this, args, cancellationToken);
                    Interlocked.Decrement(ref activeTasks);
                }, cancellationToken, TaskCreationOptions.None, taskScheduler);
            }
        }

        public async Task WaitForTasksToFinish(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && Interlocked.Read(ref activeTasks) > 0)
            {
                await Task.Delay(1000);
            }
        }
    }
}
