using System;
using System.IO;

public sealed class TemporaryFile : IDisposable
{
    public string FilePath { get; private set; }

    public TemporaryFile()
    {
        string directory = Path.GetTempPath();
        string file = Path.GetRandomFileName();
        FilePath = Path.Combine(directory, file);
    }

    public TemporaryFile(string directory)
    {
        string file = Path.GetRandomFileName();
        FilePath = Path.Combine(directory, file);
    }

    public TemporaryFile(string directory, string file)
    {
        FilePath = Path.Combine(directory, file);
    }

    public void Dispose()
    {
        Delete();
        GC.SuppressFinalize(this);
    }

    public void Create()
    {
        try
        {
            if (null != FilePath) File.Create(FilePath);
            else Console.WriteLine("You need to intialize tempfile before use");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Environment.Exit(88);
        }
    }

    private void Delete()
    {
        if (FilePath == null) return;
        File.Delete(FilePath);
        FilePath = null;
    }
}
