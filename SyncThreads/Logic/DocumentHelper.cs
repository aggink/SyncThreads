namespace SyncThreads.Logic;

using Microsoft.Office.Interop.Word;

public static class DocumentHelper
{
    private const string FilePath = "B:\\document1.docx";

    public static void CreateDocument(Application wordApp)
    {
        Console.WriteLine("Creating an empty document...");

        wordApp.Documents.Add();

        Thread.Sleep(3000);
    }

    public static void FillDocument(Application wordApp)
    {
        Console.WriteLine("Filling the document...");

        var document = wordApp.ActiveDocument;
        document.Content.Text = "Full description of the document.";

        Thread.Sleep(2000);
    }

    public static void SaveDocument(Application wordApp)
    {
        Console.WriteLine("Saving the document...");

        var document = wordApp.ActiveDocument;

        document.SaveAs(FilePath);
        document.Close();

        Thread.Sleep(1000);
    }
}
