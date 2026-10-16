# Acceptance criteria

| Scenario | Expected result | Evidence |
| --- | --- | --- |
| Seeded session loads | Six checks, three complete, two critical blockers, HOLD | Manual smoke |
| All critical items complete | CLEAR even if optional work remains | Unit + manual |
| Critical item reopened | HOLD | Unit + manual |
| Empty board or database unavailable | HOLD and clear unavailable message | Unit + manual |
| Two tabs edit same original version | Only first write succeeds; second refreshes with conflict message | SQL integration + manual |
| Successful state change | Audit entry has same state and UTC timestamp | SQL integration |
| Refresh | Reads current persisted board and most recent eight audit entries | Manual |

Manual release review: use keyboard through refresh and complete/reopen controls; inspect mobile layout; verify no horizontal page overflow; reload after mutation to confirm persistence; disconnect SQL to verify HOLD; confirm no connection string appears in user-facing errors. Automated policy tests do not substitute for these checks.

Review checklist: agreed scope → schema and SQL review → build/unit checks → SQL integration → UI/accessibility review → private repository publication → hosted CI review. Each check needs evidence before claiming completion.
