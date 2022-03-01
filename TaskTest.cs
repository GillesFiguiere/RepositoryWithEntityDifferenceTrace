using FluentAssertions;
using Xunit;

namespace Test;

public class TaskTest
{
    [Fact]
    void Should()
    {
        Task result = Run(true);
        result.IsCompleted.Should().BeTrue();
    }
    
    [Fact]
    async Task ShouldWait()
    {
        Task result = Run(false);
        result.IsCompleted.Should().BeFalse();
        await result;
        result.IsCompleted.Should().BeTrue();
    }

    async Task Run(bool value)
    {
        if (value) return;
        await Task.Delay(5000);
    }
}