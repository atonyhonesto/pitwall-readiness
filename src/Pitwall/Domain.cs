namespace Pitwall;

public record CheckItem(int Id, string Area, string Title, string Owner, bool Critical, bool Complete, byte[] Version);
public record AuditEntry(string Title, bool Complete, DateTime ChangedAt);
public static class ReleasePolicy
{
    public static bool CanRelease(IEnumerable<CheckItem> items)
    {
        var checks = items.ToArray();
        return checks.Length > 0 && checks.All(x => !x.Critical || x.Complete);
    }
}
