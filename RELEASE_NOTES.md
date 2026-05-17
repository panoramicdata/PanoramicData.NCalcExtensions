# Release Notes

Keep upcoming changes under `## Unreleased`.
`PublishWithReleaseNote.ps1` stamps that section with the next NBGV package version,
creates a fresh `## Unreleased` section, commits the file, and then calls `Publish.ps1`.

## Unreleased

## 5.8.45 - 2026-05-17
## 5.8.43 - 2026-05-16
- Added optional `outputFormat` parameter to `countBy()` supporting `'JArray'` and `'JObject'` (case-insensitive); default behavior unchanged.
- Enhanced `in()` to accept a list variable or `list()` literal as the haystack in the second parameter position; existing varargs behavior unchanged.
- Added optional `mode` parameter to `substring()` (`'Error'`, `'Empty'`, `'Null'`, `'Clip'`) for out-of-bounds start index handling; default now throws a bounds-specific error instead of a generic type-validation message.
- Fixed multi-key `orderBy()` closure capture bug where all sort keys incorrectly used the last lambda due to LINQ lazy evaluation; multi-key and `jPath`-based sorting now works correctly.

## 5.8.37 - 2026-05-15
- Breaking change: parameter metadata APIs now use `TypedDefinition` instead of `(string TypeName, string Value)` tuples, including `ExtendedExpressionDocument.Parameters` and related definition accessors.
- Added typed parameter and answer definitions using colon-based syntax such as `// x:int:10`, `// y:string?`, and `// answer:string?:null`.
- Added support for simple and nullable aliases including `string`, `string?`, `bool`, `bool?`, `int`, `int?`, `DateTime`, and `Guid`.
- Added explicit distinction between a definition being present and a value being supplied.
- Reserved `null` as the explicit null marker for typed definitions. A literal string value of `"null"` is not supported in this metadata format.
- Reduced parser allocations by moving comment parsing and document analysis to a more direct single-pass pipeline.
- Improved repeated evaluation performance for lambda, regex, and property-access helper functions by reusing parsed expressions and caching reflection/regex lookups.
- Added `PublishWithReleaseNote.ps1` to stamp and commit release notes before versioned publish flows, and updated the release process documentation.