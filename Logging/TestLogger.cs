using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Test.Logging;

internal class TestLogger : ILogger, IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TestLogger(ITestOutputHelper testOutputHelper) =>
        _testOutputHelper = testOutputHelper;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter) =>
        _testOutputHelper.WriteLine(state?.ToString());

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => this;

    public void Dispose() {}
}