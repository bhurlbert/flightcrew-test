# Survey Baseline: Flightcrew Test

Surveyed: 2026-11-21 | Scope: whole repo | Navigator

## Summary

This repository is a Markdown-only operating framework for planning and running flight-style software changes; it contains no compiled application. [`.flightcrew/manual.md`] It exposes slash-command workflows and crew agents for assignment, briefing, flight planning, execution, landing, debriefing, survey, and black-box analysis. [`.github/skills/`, `.claude/skills/`, `.github/agents/`, `.claude/agents/`]

## Entry points

| Name | Type | Path | Notes |
|---|---|---|---|
| `/fc-manual` | Skill workflow | `.github/skills/fc-manual/SKILL.md`, `.claude/skills/fc-manual/SKILL.md` | Creates or amends `.flightcrew/manual.md`. [`.github/skills/fc-manual/SKILL.md`, `.claude/skills/fc-manual/SKILL.md`] |
| `/fc-survey` | Skill workflow | `.github/skills/fc-survey/SKILL.md`, `.claude/skills/fc-survey/SKILL.md` | Maps brownfield terrain into `.flightcrew/survey/baseline.md`. [`.github/skills/fc-survey/SKILL.md`, `.claude/skills/fc-survey/SKILL.md`] |
| `/fc-flightplan` | Skill workflow | `.github/skills/fc-flightplan/SKILL.md`, `.claude/skills/fc-flightplan/SKILL.md` | Creates the flight-plan document for requested changes. [`.github/skills/fc-flightplan/SKILL.md`, `.claude/skills/fc-flightplan/SKILL.md`] |
| `/fc-route` | Skill workflow | `.github/skills/fc-route/SKILL.md`, `.claude/skills/fc-route/SKILL.md` | Plans implementation, interfaces, data, platform changes, tests, risks, and exceptions. [`.github/skills/fc-route/SKILL.md`, `.claude/skills/fc-route/SKILL.md`] |
| `/fc-briefing`, `/fc-preflight`, `/fc-assign`, `/fc-fly` | Skill workflows | `.github/skills/{fc-briefing,fc-preflight,fc-assign,fc-fly}/SKILL.md`, `.claude/skills/{fc-briefing,fc-preflight,fc-assign,fc-fly}/SKILL.md` | Resolve questions, verify readiness, assign tasks, and execute flights. [`.github/skills/fc-briefing/SKILL.md`, `.github/skills/fc-preflight/SKILL.md`, `.github/skills/fc-assign/SKILL.md`, `.github/skills/fc-fly/SKILL.md`] |
| `/fc-land`, `/fc-debrief`, `/fc-blackbox` | Skill workflows | `.github/skills/{fc-land,fc-debrief,fc-blackbox}/SKILL.md`, `.claude/skills/{fc-land,fc-debrief,fc-blackbox}/SKILL.md` | Verify landing, debrief drift, and query provenance. [`.github/skills/fc-land/SKILL.md`, `.github/skills/fc-debrief/SKILL.md`, `.github/skills/fc-blackbox/SKILL.md`] |
| Navigator, Engineer, Check Pilot | Agent definitions | `.github/agents/*.agent.md`, `.claude/agents/*.md` | Define read/survey planning, implementation, and independent verification roles. [`.github/agents/navigator.agent.md`, `.github/agents/engineer.agent.md`, `.github/agents/check-pilot.agent.md`, `.claude/agents/navigator.md`, `.claude/agents/engineer.md`, `.claude/agents/check-pilot.md`] |

## Interfaces

| Interface | Kind | Contract location | Consumers |
|---|---|---|---|
| Skill invocation | Markdown command/workflow | `.github/skills/*/SKILL.md`, `.claude/skills/*/SKILL.md` | Captain/First Officer and the available agent runtime. [`.github/skills/fc-fly/SKILL.md`, `.claude/skills/fc-fly/SKILL.md`] |
| Agent role contract | Markdown/YAML-front-matter role | `.github/agents/*.agent.md`, `.claude/agents/*.md` | Crew workflows dispatch Navigator, Engineer, or Check Pilot behavior. [`.github/agents/navigator.agent.md`, `.github/agents/engineer.agent.md`, `.github/agents/check-pilot.agent.md`] |
| Flight documents | Markdown records/templates | `.flightcrew/templates/*.md` | Flight plan, route, assignments, preflight, landing, debrief, and survey workflows. [`.flightcrew/templates/flightplan.md`, `.flightcrew/templates/route.md`, `.flightcrew/templates/assignments.md`, `.flightcrew/templates/preflight.md`, `.flightcrew/templates/landing.md`, `.flightcrew/templates/debrief.md`, `.flightcrew/templates/survey.md`] |
| Project black box | Append-only Markdown log | `.flightcrew/blackbox.md` | Manual, survey, flight, and governance provenance queries. [`.flightcrew/blackbox.md`, `.github/skills/fc-blackbox/SKILL.md`] |
| No REST, event, queue, or scheduled interface | None found | No application or API contract files | No external runtime consumers found. [`.flightcrew/manual.md`] |

## Data

| Store | Key entities | Schema / migration path | Notes |
|---|---|---|---|
| Markdown repository files | skills, agents, templates, ops manual, project/flight records | None; no migration system declared | There is no database or schema layer. [`.flightcrew/manual.md`, `.flightcrew/templates/`] |
| Project black box | timestamped project actions and approvals | `.flightcrew/blackbox.md`; append-only convention | It is currently untracked in Git and contains the manual and survey entries. [`.flightcrew/blackbox.md`] |
| Flight records | flight plan, route, briefing, assignments, preflight, landing, debrief, flight black box | `.flightcrew/flights/<NNN-short-name>/` referenced by skills, no current flight folder exists | Flight structure is operationally referenced but not present in the surveyed tree. [`.github/skills/fc-fly/SKILL.md`, `.github/skills/fc-route/SKILL.md`] |

## Dependencies

| Dependency | Purpose | Where configured | Secrets referenced (names only) |
|---|---|---|---|
| Markdown plus YAML front matter | Skill, agent, template, and project-record format | `.github/`, `.claude/`, `.flightcrew/` | None [`.flightcrew/manual.md`] |
| GitHub-hosted agent/skill conventions | GitHub-side entry points | `.github/agents/*.agent.md`, `.github/skills/*/SKILL.md` | None found [`.github/agents/navigator.agent.md`, `.github/skills/fc-survey/SKILL.md`] |
| Claude-hosted skill/agent conventions | Claude-side entry points | `.claude/agents/*.md`, `.claude/skills/*/SKILL.md` | None found [`.claude/agents/navigator.md`, `.claude/skills/fc-survey/SKILL.md`] |
| Git | History and working-tree status | Repository metadata | None found [`.flightcrew/manual.md`] |
| No package, SDK, application framework, or CI dependency manifest found | Not applicable | No dependency manifest or CI workflow in the repository | None found [`.flightcrew/manual.md`] |

## Tests

- Frameworks: No test framework or test files declared or present. [`.flightcrew/manual.md`]
- What runs in CI: No CI workflow found; none is declared in the ops manual. [`.flightcrew/manual.md`]
- Well covered: No implementation behavior is covered by repository tests. [`.flightcrew/manual.md`]
- Untested or thin: Markdown links, mirror consistency, front-matter validity, slash-command behavior, and template completeness have no automated checks. (inferred from the absence of test or lint configuration in `.flightcrew/manual.md`)

## Hot spots

| Path | Why it is hot (size, churn, TODOs, fragility) |
|---|---|
| `.github/skills/*/SKILL.md`, `.claude/skills/*/SKILL.md` | The repository maintains two skill trees; all 11 skill pairs were checked and match byte-for-byte, but future edits could silently diverge. [`.github/skills/`, `.claude/skills/`] |
| `.github/agents/*.agent.md`, `.claude/agents/*.md` | Two agent mirrors use different filenames and tool-front-matter shapes, creating version-drift and discovery risk. [`.github/agents/navigator.agent.md`, `.claude/agents/navigator.md`, `.github/agents/engineer.agent.md`, `.claude/agents/engineer.md`, `.github/agents/check-pilot.agent.md`, `.claude/agents/check-pilot.md`] |
| `.flightcrew/manual.md` | It is central to every quality gate and guardrail, but it is currently untracked and has no checked-in project manual history entry. [`.flightcrew/manual.md`, `.flightcrew/blackbox.md`] |
| `.flightcrew/blackbox.md` | It is the append-only provenance record, but it is currently untracked in Git. [`.flightcrew/blackbox.md`] |
| `.flightcrew/templates/` | A small template change can affect every future flight document; template completeness is the de facto contract but no validation exists. [`.flightcrew/templates/survey.md`, `.flightcrew/templates/route.md`, `.flightcrew/templates/flightplan.md`, `.flightcrew/manual.md`] |

## Hazards

1. **No automated verification.** There is no build, test, lint, coverage, security, or CI gate. The only required available gate is `git diff --check`. [`.flightcrew/manual.md`]
2. **Mirror drift across GitHub and Claude definitions.** Skills are currently mirrored exactly, while agent declarations use platform-specific filenames and front-matter shapes. [`.github/`, `.claude/`]
3. **Untracked project operating records.** `.flightcrew/manual.md` and `.flightcrew/blackbox.md` are not in the checked-in baseline, so provenance and standards may disappear across clean checkouts. [`.flightcrew/manual.md`, `.flightcrew/blackbox.md`]
4. **No flight or implementation data exists yet.** The workflows are documented, but their runtime behavior and end-to-end handoffs are untested. [`.github/skills/fc-fly/SKILL.md`, `.github/skills/fc-preflight/SKILL.md`, `.github/skills/fc-land/SKILL.md`]
5. **Guardrail exceptions are easy to miss in Markdown.** Enforcement depends on reading skill and manual text rather than executable checks. [`.flightcrew/manual.md`, `.github/skills/fc-route/SKILL.md`, `.github/skills/fc-fly/SKILL.md`]

## Alternatives considered

- Alternative: Re-run the detailed survey only for `.github/` and treat `.claude/` and `.flightcrew/` as configuration. It was rejected because the requested scope is whole-repository and these trees are distinct entry-point and operating-record surfaces. [`.github/`, `.claude/`, `.flightcrew/`]
- Alternative: Infer a data store or runtime dependency from the workflow terminology. It was rejected because no manifest, schema, API, or implementation file is present. [`.flightcrew/manual.md`, `.github/skills/fc-fly/SKILL.md`]

## Inferred (unverified)

- The repository is intended primarily as a distributable prompt/agent configuration rather than an executable service. (inferred from the Markdown-only layout and the mission in `.flightcrew/manual.md`)
- The duplicated `.github/` and `.claude/` trees are intended to be kept semantically aligned. (inferred from identical skill contents and matching agent role names in `.github/` and `.claude/`)
- A future change should validate copied skill definitions and front matter before adding executable tooling. (inferred from the no-build, no-test baseline in `.flightcrew/manual.md`)
