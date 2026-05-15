# Release Notes

Keep upcoming changes under `## Unreleased`.
`PublishWithReleaseNote.ps1` stamps that section with the next NBGV package version,
creates a fresh `## Unreleased` section, commits the file, and then calls `Publish.ps1`.

## Unreleased

## 5.8.37 - 2026-05-15
- Breaking change: parameter metadata APIs now use `TypedDefinition` instead of `(string TypeName, string Value)` tuples, including `ExtendedExpressionDocument.Parameters` and related definition accessors.
- Added typed parameter and answer definitions using colon-based syntax such as `// x:int:10`, `// y:string?`, and `// answer:string?:null`.
- Added support for simple and nullable aliases including `string`, `string?`, `bool`, `bool?`, `int`, `int?`, `DateTime`, and `Guid`.
- Added explicit distinction between a definition being present and a value being supplied.
- Reserved `null` as the explicit null marker for typed definitions. A literal string value of `"null"` is not supported in this metadata format.
- Reduced parser allocations by moving comment parsing and document analysis to a more direct single-pass pipeline.
- Improved repeated evaluation performance for lambda, regex, and property-access helper functions by reusing parsed expressions and caching reflection/regex lookups.
- Added `PublishWithReleaseNote.ps1` to stamp and commit release notes before versioned publish flows, and updated the release process documentation.