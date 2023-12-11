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
            semaphore.Wait();

            while (threadId != coordinator.CurrentThread)
            {
                semaphore.Release();

                Thread.Sleep(10);

                semaphore.Wait();
            }

            Console.WriteLine($"{threadName}: Acquired Coordinator, executing...");

            action(_wordApp);

            coordinator.CurrentThread = (coordinator.CurrentThread % 3) + 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{threadName} Exception: {ex}");
        }
        finally
        {
            Console.WriteLine($"{threadName}: Released Coordinator.");

            semaphore.Release();
        }
    }
}
