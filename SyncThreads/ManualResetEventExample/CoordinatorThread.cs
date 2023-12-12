using SyncThreads.Logic;

namespace SyncThreads.ManualResetEventExample;

/*
 * ManualResetEvent используется для сигнализации и ожидания событий.
 * Каждый поток ожидает сигнала соответствующего события перед выполнением, а затем сигнализирует о следующем событии в строке после завершения.
 * Это гарантирует, что потоки выполняются в желаемом порядке.
*/

public sealed class CoordinatorThread : IDisposable
{
    private readonly ThreadWorker _threadWorker = new ThreadWorker();
    private readonly ManualResetEvent _threadEvent_1 = new ManualResetEvent(true);
    private readonly ManualResetEvent _threadEvent_2 = new ManualResetEvent(false);
    private readonly ManualResetEvent _threadEvent_3 = new ManualResetEvent(false);

    public void Run()
    {
        var thread1 = Task.Run(() => _threadWorker.Execute("Thread 1 - Create document", _threadEvent_1, _threadEvent_2, DocumentHelper.CreateDocument));
        var thread2 = Task.Run(() => _threadWorker.Execute("Thread 2 - Fill document", _threadEvent_2, _threadEvent_3, DocumentHelper.FillDocument));
        var thread3 = Task.Run(() => _threadWorker.Execute("Thread 3 - Save document", _threadEvent_3, _threadEvent_1, DocumentHelper.SaveDocument));

        Task.WhenAll(thread1, thread2, thread3).Wait();
    }

    public void Dispose()
    {
        _threadEvent_1.Dispose();
        _threadEvent_2.Dispose();
        _threadEvent_3.Dispose();
    }
}
