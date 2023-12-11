using Microsoft.Office.Interop.Word;

namespace SyncThreads.ManualResetEventExample;

public sealed class ThreadWorker
{
    private readonly Application _wordApp = new Application();

    public void Execute(string threadName, ManualResetEvent currentEvent, ManualResetEvent nextEvent, Action<Application> action)
    {
        try
        {
            currentEvent.WaitOne();

            Console.WriteLine($"{threadName}: Acquired Coordinator, executing...");

            action(_wordApp);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{threadName} Exception: {ex}");
        }
        finally
        {
            Console.WriteLine($"{threadName}: Released Coordinator.");

            currentEvent.Reset();
            nextEvent.Set();
        }
    }
}