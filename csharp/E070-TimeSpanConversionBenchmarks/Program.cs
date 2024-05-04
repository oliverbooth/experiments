using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<TimeSpanConversionBenchmarks>();

[SimpleJob, MemoryDiagnoser(false)]
public class TimeSpanConversionBenchmarks
{
    private readonly long[] _ticks = new long[1_000_000];

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random();

        for (var i = 0; i < _ticks.Length; i++)
        {
            _ticks[i] = random.NextInt64();
        }
    }

    [Benchmark]
    public TimeSpan[] UsingConstructor()
    {
        var times = new TimeSpan[_ticks.Length];

        for (var i = 0; i < _ticks.Length; i++)
        {
            times[i] = new TimeSpan(_ticks[i]);
        }

        return times;
    }

    [Benchmark]
    public TimeSpan[] UsingUnsafe()
    {
        var times = new TimeSpan[_ticks.Length];

        unsafe
        {
            fixed (long* l = _ticks)
            fixed (TimeSpan* ts = times)
            {
                long* pTicks = l;
                TimeSpan* pTimeSpan = ts;

                for (var i = 0; i < _ticks.Length; i++)
                {
                    *pTimeSpan = Unsafe.As<long, TimeSpan>(ref *pTicks);
                    pTicks++;
                    pTimeSpan++;
                }
            }
        }

        return times;
    }

    [Benchmark]
    public TimeSpan[] UsingRealitysUnsafe()
    {
        var times = new TimeSpan[_ticks.Length];

        unsafe
        {
            fixed (long* l = _ticks)
            fixed (TimeSpan* ts = times)
            {
                for (var i = 0; i < _ticks.Length; i++)
                {
                    Unsafe.Write(ts + i, *(l + i));
                }
            }
        }

        return times;
    }

    [Benchmark]
    public TimeSpan[] UsingMemoryMarshal()
    {
        var times = new TimeSpan[_ticks.Length];
        _ticks.CopyTo(MemoryMarshal.Cast<TimeSpan, long>(times));
        return times;
    }
}
