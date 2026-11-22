# Ops Manual: Flightcrew Test

Version: 1.0 | Last amended: 2026-11-21 | Captain: pending

## Mission
This repository defines and stores the Flightcrew Markdown skills, agents, templates, and project operating records.
It provides repeatable planning, implementation, verification, landing, and debrief workflows rather than a compiled application.

## Principles
1. Every flight change must trace to a requirement or acceptance check and to a recorded task.
2. Route changes touching guardrails require explicit captain approval in the route and task record.
3. Engineers must stay inside their assigned task; changes outside the route require a stop and captain decision.
4. Check Pilot verification must be independent from the Engineer that built the work.
5. Every completed task must leave a passing, command-backed quality evidence trail.
6. Secrets and production values must never be read, copied, or recorded; reference names and locations only.
7. Existing Flightcrew patterns take precedence over introducing new frameworks or conventions.

## Tech standards
- Languages and versions: Markdown; YAML front matter in `.github/` and `.claude/` skill and agent definitions. No language runtime or package version is declared.
- Frameworks and libraries: No application framework or dependency manifest is present.
- Patterns to use: Existing skill `SKILL.md` front matter, agent Markdown definitions, `.flightcrew/templates/` documents, append-only project and flight black boxes.
- Patterns to avoid: Inventing build systems, dependencies, framework conventions, or acceptance evidence without repository or captain authorization.
- Style and formatting: UTF-8 Markdown, concise headings and lists, relative repository paths in records, and append-only timestamps in ISO 8601 form.

## Quality gates
| Gate | Command | Required |
|---|---|---|
| Whitespace and conflict markers | `git diff --check` | yes |
| Build | Not available: no build manifest or build workflow found | no |
| Unit tests | Not available: no test setup or test files found | no |
| Lint | Not available: no lint configuration found | no |
| Security scan | Not available: no scanner or CI workflow found | no |
| Scope and trace review | Compare changed files with the route and report unplanned changes | yes |

Run the available gate on every change. If a future change introduces code or a build/test/lint system, add the exact commands to this manual before landing that change.

## Guardrails
- Schema or data migrations require captain approval.
- Reading, writing, or moving secrets requires captain approval and is prohibited from output or black box entries.
- Production configuration and production access require captain approval.
- Deleting or moving files requires captain approval.
- External services, package installations, and repository rewrites require captain approval unless explicitly in an approved flight.
- Work outside an assigned route requires a stop and route amendment approved by the captain.

## Crew rules
- Seats in use: captain, first officer, navigator, engineer, check-pilot.
- Max parallel engineer tasks: 1; a `[P]` task may run only when it shares no files and no dependency with another task.
- The Engineer may build only one assigned task at a time and never verify high-risk work it built.
- The Check Pilot is independent and uses evidence, commands, or described manual verification for every verdict.
- The Captain must decide guardrail exceptions, approval decisions, and unresolved flight requirements.
- The First Officer maintains the flight plan, assignments, and project black box.

## Definition of done
- Every acceptance check is mapped to a task and independently verified.
- `git diff --check` passes.
- Changed files match the approved route; unplanned changes are explained or removed.
- The flight and project black boxes contain append-only evidence for the change.
- A Captain-approved pull request ships the change for shipped work.
- Repository-specific build, test, lint, coverage, or security gates apply only when that tooling exists and is recorded here.

## Changelog
| Date | Change | Approved by |
|---|---|---|
| 2026-11-21 | Initial manual | pending |
