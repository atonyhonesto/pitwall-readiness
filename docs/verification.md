# Verification record — October 16, 2026

## Executed and passed

- Release build with warnings as errors: **0 warnings, 0 errors**.
- Final NuGet scan of direct and transitive dependencies: **no vulnerable packages reported** by the configured sources. A transitive Pkcs advisory was resolved by explicitly referencing 8.0.2; restore now treats NU1901–NU1904 advisories as errors.
- **5 tests passed, 0 failed, 0 skipped** using the .NET 8.0.32 runtime. Build SDK: 10.0.400, target framework net8.0.
- Four unit cases cover an empty board, critical blockers, optional work, and reopening critical work.
- SQL integration: successful update, rejection of stale rowversion, persisted changed state, matching audit state/title, and restoration of original check state.
- Database engine used locally: SQL Server 2025 Express LocalDB 17.0.4025.3. Dedicated database: `PitwallReadiness_Demo_20261016`.
- Browser: seeded board shows 3/6 complete and two blockers; completing the blockers changes HOLD to CLEAR; reload retains changes; reopening returns HOLD; a stale second tab receives the conflict notice.
- Responsive views inspected at 390 and 1280 CSS-pixel viewport settings. Mobile document width equals scroll width (375), with no horizontal overflow. Desktop preview saved as `preview.jpg`.
- Sample checklist state restored to 3/6 complete, two blockers after browser checks. Audit records remain as demonstration evidence.

## Issue found and fixed

The first SQL integration run rejected direct OUTPUT INTO an audit table with a foreign key. The implementation now captures changed rows into a table variable and inserts audit records within an explicit transaction, preserving the foreign key. All five tests passed after the fix.

## Not yet confirmed

- Hosted GitHub Actions has not run. The SQL Server 2022 container job is defined but local tests used SQL Server 2025 LocalDB.
- GitHub repository creation/push: pending account connection. Requested destination name: `pitwall-readiness`, **private**.
- Full accessibility audit, database-outage browser exercise, and production deployment checks remain outside the executed checks above.

The application is a local portfolio demo with no authentication or verified audit actor. See the README deployment boundary before hosting.
