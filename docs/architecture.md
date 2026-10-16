# Decisions

1. **Interactive Server Blazor, .NET 8.** C# end to end, small deployment footprint, server-side database access. Persistent circuits require connection-aware hosting; scale-out would need deliberate circuit and data protection planning.
2. **Parameterized ADO.NET.** Explicit SQL makes constraints, update semantics, and transaction boundaries reviewable. It also makes reader mappings our responsibility. No EF package, generated migrations, or hidden repository framework.
3. **One atomic transaction.** UPDATE OUTPUT captures changed rows in a table variable; an INSERT copies them to Audit before COMMIT. XACT_ABORT rolls back runtime SQL failures. This preserves the audit foreign key and prevents a successful checklist change without its audit entry. Rowversion detects concurrent edits, including changes made and then reverted.
4. **Derived gate.** No duplicated release flag to drift from checklist truth. Empty boards fail closed. This demonstrates a workflow, not a substitute for safety sign-off.
5. **Bounded sample.** One seeded session, predefined checks and owner roles. No identity, CRUD administration, telemetry, AI runtime calls, multi-event filtering, or deployment automation. These are explicit future slices, not hidden claims.

Errors are logged server-side with generic UI messages. Read failures clear the board so stale data cannot show CLEAR. Update failures ask the operator to refresh because a disconnected response cannot prove whether SQL committed. Audit timestamps are UTC; they are not claimed to be verified human sign-offs.

Dependency review found SqlClient resolved System.Security.Cryptography.Pkcs 8.0.1 transitively. The app explicitly pins 8.0.2, the patched .NET 8 version in [Microsoft's advisory](https://github.com/advisories/GHSA-846f-f764-76mx). NuGet auditing includes transitive dependencies and treats vulnerability warnings as errors during restore.

Schema changes should be additive where feasible, reviewed against a backup, tested on representative data and given a rollback/forward-repair plan. The initial scripts only provision a new schema and sample data; they are not a general migration engine.
