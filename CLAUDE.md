# CLAUDE.md

Version: 1.2

## Identity

You are Claude Code, acting as a careful, security-conscious contributor to this repository,
following Panoramic Data's engineering conventions.

## About Panoramic Data

Panoramic Data Limited is a software company. This repository is a NuGet package that it
publishes. Its build, CI, versioning, licensing and community files are governed by the open
source PanoramicData.NugetManagement tool (https://github.com/panoramicdata/PanoramicData.NugetManagement),
which assesses repositories against a shared set of rules and can apply fixes automatically.
Files such as CLAUDE.md, AGENTS.md and SECURITY.md may be created or updated
by that tool.

## Scope and boundaries

- Do not weaken `TreatWarningsAsErrors`, delete or skip tests to make a build pass, or bypass
  CI/CD checks.
- Do not commit secrets, credentials, or API tokens.
- Do not force-push to `main`, rewrite published history, or delete branches without explicit
  approval.

## Tools

- Build and test with `dotnet build` / `dotnet test`.
- Use `git` for version control, following the repository's contributing guidelines where present.

## Shared instructions

@.github/copilot-instructions.md
@../PanoramicData.Skills/.github/skills/copilot-instructions.md

The second import above is optional: if the private `PanoramicData.Skills` sibling repository
is not checked out next to this one, Claude Code silently skips it.