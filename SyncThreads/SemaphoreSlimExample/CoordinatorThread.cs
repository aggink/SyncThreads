using SyncThreads.Logic;

namespace SyncThreads.SemaphoreSlimExample;

/*
 * Введена переменная currentThread, чтобы отслеживать, какой поток должен выполниться следующим. Каждый поток проверяет, пришла ли его очередь выполняться. 
 * Если нет, он освобождает семафор и некоторое время ждет перед повторной проверкой. Это должно помочь предотвратить взаимоблокировки и обеспечить строгое чередование потоков.
 */

public sealed class CoordinatorThread : IDisposable
{
    private int _currentThread = 1;

    private readonly ThreadWorker _threadWorker = new ThreadWorker();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public int CurrentThread
    {
        get => _currentThread;
        set => _currentThread = value;
    }
    
    public SemaphoreSlim Semaphore => _semaphore;

    public void Run()
    {
        var thread1 = new Thread(() => _threadWorker.Execute("Thread 1 - Create document", 1, this, DocumentHelper.CreateDocument));
        var thread2 = new Thread(() => _threadWorker.Execute("Thread 2 - Fill document", 2, this, DocumentHelper.FillDocument));
        var thread3 = new Thread(() => _threadWorker.Execute("Thread 3 - Save document", 3, this, DocumentHelper.SaveDocument));

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
