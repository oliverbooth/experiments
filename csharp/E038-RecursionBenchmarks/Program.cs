using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RecursionBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class RecursionBenchmarks
{
    [Benchmark]
    [Arguments("C:\\Mame")]
    public void ListFilesWithRecursion(string path)
    {
        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                // print filename
            }

            foreach (string subDir in Directory.GetDirectories(path))
                ListFilesWithRecursion(subDir);
        }
        catch (UnauthorizedAccessException)
        {
            // ignored
        }
    }

    [Benchmark]
    [Arguments("C:\\Mame")]
    public void ListFilesWithIteration(string path)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(path);

        while (queue.Count > 0)
        {
            string currentDir = queue.Dequeue();
            try
            {
                foreach (string file in Directory.GetFiles(currentDir))
                {
                    // print filename
                }

                foreach (string subDir in Directory.GetDirectories(currentDir))
                    queue.Enqueue(subDir);
            }
            catch (UnauthorizedAccessException)
            {
                // ignored
            }
        }
    }
}
