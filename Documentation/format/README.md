# format()

| Field | Value |
| --- | --- |
| Purpose | Formats strings and numbers as output strings with the specified format. |
| Parameters | * object (number or text) * format: the format to use - see C# number and date/time formatting - weekOfMonth is the numeric week of month as would be shown on a calendar with one row per week with weeks starting on a Sunday - weekOfMonthText is the same as weekOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last' - weekDayOfMonth is the number of times this weekday has occurred within the month so far, including this one - weekDayOfMonthText is the same as weekDayOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last' - weekOfYear is the culture-specific week number of the year (1-53) - isoWeekOfYear is the ISO 8601 week number of the year (1-53) * timeZone [optional] - see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0 |
| Examples | 13 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | format(1, '00') | string | '01' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-01.ncalc) |
| 2 | format(1.0, '00') | string | '01' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-02.ncalc) |
| 3 | format('2021-11-29', 'dayOfYear') | string | '333' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-03.ncalc) |
| 4 | format('2021-11-01', 'weekOfMonth') | int | 1 | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-04.ncalc) |
| 5 | format('2021-11-01', 'weekOfMonthText') | string | 'first' | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-05.ncalc) |
| 6 | format('2021-11-28', 'weekOfMonth') | int | 5 | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-06.ncalc) |
| 7 | format('2021-11-28', 'weekOfMonthText') | string | 'last' | [example-07.ncalc](example-07.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-07.ncalc) |
| 8 | format('2021-11-28', 'weekDayOfMonth') | int | 4 | [example-08.ncalc](example-08.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-08.ncalc) |
| 9 | format('2021-11-28', 'weekDayOfMonthText') | string | 'forth' | [example-09.ncalc](example-09.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-09.ncalc) |
| 10 | format('2024-01-01', 'weekOfYear') | string | '1' (can be controlled with CultureOptions, but defaults to CultureInfo.Invariant) | [example-10.ncalc](example-10.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-10.ncalc) |
| 11 | format('2024-01-01', 'isoWeekOfYear') | string | '1' | [example-11.ncalc](example-11.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-11.ncalc) |
| 12 | format('01/01/2019', 'yyyy-MM-dd') | string | '2019-01-01' | [example-12.ncalc](example-12.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-12.ncalc) |
| 13 | format(theDateTime, 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') [where theDateTime is a .NET DateTime, set to DateTime.Parse("2020-03-13 16:00", CultureInfo.InvariantCulture)] | string | '2020-03-13 12:00' | [example-13.ncalc](example-13.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fformat%2Fexample-13.ncalc) |

