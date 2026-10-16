using Microsoft.Extensions.Configuration;
using Pitwall;
using Xunit;

public class ReadinessTests
{
    static CheckItem Check(bool critical, bool complete) => new(1,"Garage","Torque","Mechanic",critical,complete,[]);
    [Fact] public void EmptyBoardHoldsRelease() => Assert.False(ReleasePolicy.CanRelease([]));
    [Fact] public void IncompleteCriticalItemHoldsRelease() => Assert.False(ReleasePolicy.CanRelease([Check(true,true), Check(true,false)]));
    [Fact] public void OptionalWorkDoesNotBlockRelease() => Assert.True(ReleasePolicy.CanRelease([Check(true,true), Check(false,false)]));
    [Fact] public void ReopeningCriticalWorkHoldsRelease() => Assert.False(ReleasePolicy.CanRelease([Check(true,false),Check(false,true)]));

    [Fact]
    [Trait("Category", "SqlIntegration")]
    public async Task SqlUpdateIsAuditedAndRejectsStaleToken()
    {
        var connection = Environment.GetEnvironmentVariable("PITWALL_TEST_SQL");
        // Separate integration CI job provides this variable; normal unit runs exclude this category.
        Assert.False(string.IsNullOrWhiteSpace(connection), "PITWALL_TEST_SQL must target a dedicated seeded test database.");
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string?> { ["ConnectionStrings:Pitwall"] = connection }).Build();
        var store = new ReadinessStore(config);
        var original = (await store.GetChecksAsync()).First();
        Assert.True(await store.SetCompleteAsync(original.Id, original.Version, !original.Complete));
        Assert.False(await store.SetCompleteAsync(original.Id, original.Version, original.Complete));
        var refreshed = (await store.GetChecksAsync()).Single(x => x.Id == original.Id);
        Assert.Equal(!original.Complete, refreshed.Complete);
        var entry = (await store.GetAuditAsync()).First();
        Assert.Equal(original.Title, entry.Title);
        Assert.Equal(!original.Complete, entry.Complete);
        Assert.True(await store.SetCompleteAsync(refreshed.Id, refreshed.Version, original.Complete));
    }
}
