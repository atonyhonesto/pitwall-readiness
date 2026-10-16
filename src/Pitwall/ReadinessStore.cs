using System.Data;
using Microsoft.Data.SqlClient;

namespace Pitwall;

public sealed class ReadinessStore(IConfiguration configuration)
{
    private SqlConnection Connection() => new(configuration.GetConnectionString("Pitwall")
        ?? throw new InvalidOperationException("Configure ConnectionStrings:Pitwall before starting the board."));

    public async Task<List<CheckItem>> GetChecksAsync()
    {
        await using var connection = Connection();
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT Id, Area, Title, Owner, Critical, Complete, Version FROM dbo.Checks ORDER BY Critical DESC, Id", connection);
        await using var reader = await command.ExecuteReaderAsync();
        var items = new List<CheckItem>();
        while (await reader.ReadAsync()) items.Add(new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetBoolean(4), reader.GetBoolean(5), (byte[])reader[6]));
        return items;
    }

    public async Task<bool> SetCompleteAsync(int id, byte[] version, bool complete)
    {
        await using var connection = Connection();
        await connection.OpenAsync();
        await using var command = new SqlCommand("""
            SET XACT_ABORT ON;
            DECLARE @changed TABLE (CheckId int, Complete bit);
            BEGIN TRANSACTION;
            UPDATE dbo.Checks SET Complete = @complete
            OUTPUT inserted.Id, inserted.Complete INTO @changed
            WHERE Id = @id AND Version = @version;
            INSERT dbo.Audit(CheckId, Complete, ChangedAt)
            SELECT CheckId, Complete, SYSUTCDATETIME() FROM @changed;
            COMMIT;
            SELECT COUNT(*) FROM @changed;
            """, connection);
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@version", SqlDbType.Timestamp).Value = version;
        command.Parameters.Add("@complete", SqlDbType.Bit).Value = complete;
        return (int)(await command.ExecuteScalarAsync())! == 1;
    }

    public async Task<List<AuditEntry>> GetAuditAsync()
    {
        await using var connection = Connection();
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT TOP (8) c.Title, a.Complete, a.ChangedAt FROM dbo.Audit a JOIN dbo.Checks c ON c.Id=a.CheckId ORDER BY a.Id DESC", connection);
        await using var reader = await command.ExecuteReaderAsync();
        var entries = new List<AuditEntry>();
        while (await reader.ReadAsync()) entries.Add(new(reader.GetString(0), reader.GetBoolean(1), DateTime.SpecifyKind(reader.GetDateTime(2), DateTimeKind.Utc)));
        return entries;
    }
}
