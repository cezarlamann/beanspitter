namespace BeanSpitter.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventRiserClass<T> where T : EventArgs
    {
        void RaiseEventsOnThreadPool(Func<object, T, CancellationToken, Task> evt, T args, CancellationToken cancellationToken);
        Task WaitForTasksToFinish(CancellationToken cancellationToken);
    }
}
