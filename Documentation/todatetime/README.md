# toDateTime()

| Field | Value |
| --- | --- |
| Purpose | Converts a string to a UTC DateTime. May take an optional inputTimeZone. |
| Parameters | * inputString * stringFormat * inputTimeZone (optional) See https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0 |
| Notes | When using numbers as the first input parameter, provide it as a decimal (see examples, below) to avoid hitting an NCalc bug relating to longs being interpreted as floats. |
| Examples | 6 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | toDateTime('2019-01-01', 'yyyy-MM-dd') | System.DateTime | A date time representing 2019-01-01 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-01.ncalc) |
| 2 | toDateTime('2020-02-29 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') | System.DateTime | A date time representing 2020-02-29 17:00:00 UTC | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-02.ncalc) |
| 3 | toDateTime('2020-03-13 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') | System.DateTime | A date time representing 2020-03-13 16:00:00 UTC | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-03.ncalc) |
| 4 | toDateTime(161827200.0, 's', 'UTC') | System.DateTime | A date time representing 1975-02-17 00:00:00 UTC | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-04.ncalc) |
| 5 | toDateTime(156816000000.0, 'ms', 'UTC') | System.DateTime | A date time representing 1974-12-21 00:00:00 UTC | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-05.ncalc) |
| 6 | toDateTime(156816000000000.0, 'us', 'UTC') | System.DateTime | A date time representing 1974-12-21 00:00:00 UTC | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftodatetime%2Fexample-06.ncalc) |

