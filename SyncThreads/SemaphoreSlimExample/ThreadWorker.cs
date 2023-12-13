using Microsoft.Office.Interop.Word;

namespace SyncThreads.SemaphoreSlimExample;

public sealed class ThreadWorker
{
    private readonly Application _wordApp = new Application();

    public void Execute(string threadName, int threadId, CoordinatorThread coordinator, Action<Application> action)
    {
        var semaphore = coordinator.Semaphore;

        try
        {
            Console.WriteLine($"{threadName}: Waiting...");

            semaphore.Wait();

            while (threadId != coordinator.CurrentThread)
            {
                Console.WriteLine($"{threadName}: {threadId} != {coordinator.CurrentThread}");
                
                semaphore.Release();

                Thread.Sleep(10);

                Console.WriteLine($"{threadName}: Waiting...");

                semaphore.Wait();
            }

            Console.WriteLine($"{threadName}: Executing...");

            action(_wordApp);

            coordinator.CurrentThread = (coordinator.CurrentThread % coordinator.ThreadCount) + 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{threadName} Exception: {ex}");
        }
        finally
        {
            Console.WriteLine($"{threadName}: Сompleted.");

            semaphore.Release();
        }
    }
}
