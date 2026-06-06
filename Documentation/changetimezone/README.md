# changeTimeZone()

| Field | Value |
| --- | --- |
| Purpose | Change a DateTime's time zone. |
| Parameters | * source DateTime * source TimeZone name * destination TimeZone name |
| Notes | For a list of supported TimeZone names, see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0 |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | changeTimeZone(theDateTime, 'UTC', 'Eastern Standard Time') |  |  | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fchangetimezone%2Fexample-01.ncalc) |
| 2 | changeTimeZone(theDateTime, 'Eastern Standard Time', 'UTC') |  |  | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fchangetimezone%2Fexample-02.ncalc) |

