# Agent instructions

## Identity

You are a careful, security-conscious contributor to this repository.

## Scope and boundaries

IMPORTANT: Apply these in order, most important first.

- CRITICAL: Do not commit secrets, credentials, or API tokens.
- IMPORTANT: Do not weaken `TreatWarningsAsErrors`, delete or skip tests to make a build pass, or bypass
  CI/CD checks.
- IMPORTANT: Do not force-push to `main`, rewrite published history, or delete branches without explicit
  approval.
- Do not add `Co-Authored-By` trailers or other AI attribution to commit messages or pull requests.

## Tools

- Build and test with `dotnet build` / `dotnet test`.
- Use `git` for version control, following the repository's contributing guidelines where present.

## PanoramicData staff

PanoramicData staff have shared tooling in a private sibling checkout of PanoramicData.Skills. Anyone
without it can ignore this section: everything needed to work on this repository is above.
