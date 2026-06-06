# timeSpan()

| Field | Value |
| --- | --- |
| Purpose | Determines the amount of time between two DateTimes. The following units are supported: * Years * Weeks * Days * Hours * Minutes * Seconds * Milliseconds * Any other string is handled with TimeSpan.ToString(timeUnit). See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings |
| Parameters | * startDateTime * endDateTime * timeUnit |
| Examples | 1 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | timeSpan('2019-01-01 00:01:00', '2019-01-01 00:02:00', 'seconds') | int | 3600 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftimespan%2Fexample-01.ncalc) |

