using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Logging;
using Test.Persistence;
using Test.Persistence.Entities;
using Test.Persistence.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace Test;

public class RepositoryWithEntityDifferenceTraceTest
{
    [Fact]
    void ShouldTraceDifferences()
    {
        // GIVEN
        using var context = new Context(
            new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase("Test")
                .Options);

        var logger = new TestLogger(_testOutputHelper);

        var repo = new RepositoryWithEntityDifferenceTrace<Attachment>(new Repository<Attachment>(context), logger);

        var id = Guid.NewGuid();
        
        // WHEN
        repo.CreateOrUpdate(new Attachment { Id = id, FileName = "Initial.pdf" });
        repo.CreateOrUpdate(new Attachment { Id = id, FileName = "NewName.pdf" });
        
        // THEN
        repo.Get(id).Should().BeEquivalentTo(new Attachment { Id = id, FileName = "NewName.pdf" });
    }
    
    private readonly ITestOutputHelper _testOutputHelper;

    public RepositoryWithEntityDifferenceTraceTest(ITestOutputHelper testOutputHelper) =>
        _testOutputHelper = testOutputHelper;
}