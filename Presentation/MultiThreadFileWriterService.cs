using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GemicleAppTest.Presentation;

public class MultiThreadFileWriterService
{
    private static ConcurrentQueue<string> _textToWrite = new ConcurrentQueue<string>();
    private CancellationTokenSource _source = new CancellationTokenSource();
    private CancellationToken _token;
    private string _path;
 
    public MultiThreadFileWriterService(string path)
    {
        _path = path;
        _token = _source.Token;
        Task.Run(WriteToFile, _token);
    }
 
    public void WriteLine(string line)
    {
        _textToWrite.Enqueue(line);
    }
 
    private async void WriteToFile()
    {
        while (true)
        {
            if (_token.IsCancellationRequested)
            {
                return;
            }
            using (StreamWriter w = File.AppendText(_path))
            {
                while (_textToWrite.TryDequeue(out string textLine))
                {
                    await w.WriteLineAsync(textLine);
                }
                w.Flush();
                Thread.Sleep(100);
            }
        }
    }
}