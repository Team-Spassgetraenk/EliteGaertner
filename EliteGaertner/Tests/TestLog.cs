using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

//Das ist nur eine Helferklasse weil alle Tests nun einen Logger verlangen
public static class TestLog
{
    public static ILogger<T> Logger<T>() => NullLogger<T>.Instance;
    public static ILoggerFactory Factory() => NullLoggerFactory.Instance;
}