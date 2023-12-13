using SyncThreads.Logic;

namespace SyncThreads.SemaphoreSlimExample;

/*
 * Введена переменная currentThread, чтобы отслеживать, какой поток должен выполниться следующим. Каждый поток проверяет, пришла ли его очередь выполняться. 
 * Если нет, он освобождает семафор и некоторое время ждет перед повторной проверкой. Это должно помочь предотвратить взаимоблокировки и обеспечить строгое чередование потоков.
 */

public sealed class CoordinatorThread : IDisposable
{
    private int _currentThread = 1;
    private int _threadCount = 0;

    private readonly ThreadWorker _threadWorker = new ThreadWorker();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public int CurrentThread
    {
        get => _currentThread;
        set => _currentThread = value;
    }

    public int ThreadCount => _threadCount;
    
    public SemaphoreSlim Semaphore => _semaphore;

    public void Run()
    {
        _threadCount = 3;

        var thread1 = Task.Run(() => _threadWorker.Execute("Thread 1 - Create document", 1, this, DocumentHelper.CreateDocument));
        var thread2 = Task.Run(() => _threadWorker.Execute("Thread 2 - Fill document", 2, this, DocumentHelper.FillDocument));
        var thread3 = Task.Run(() => _threadWorker.Execute("Thread 3 - Save document", 3, this, DocumentHelper.SaveDocument));
        
        Task.WhenAll(thread1, thread2, thread3).Wait();
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
