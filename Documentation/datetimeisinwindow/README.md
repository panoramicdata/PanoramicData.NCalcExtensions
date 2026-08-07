# dateTimeIsInWindow()

| Field | Value |
| --- | --- |
| Purpose | Returns whether the current time is within a recurring time window that starts at each fire time of a CRON expression and lasts for a duration in seconds, with an optional timezone. The window includes its start instant and excludes its end instant. |
| Parameters | * The CRON expression defining the window start times: 5 fields, or 6 fields when including seconds ('?' is accepted as '*'; day names such as SUN are recommended for the day-of-week field) * the window duration in seconds (must be positive) * optionally, the name of the timezone in which the CRON expression fires (UTC when omitted) |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | dateTimeIsInWindow('* * * * *', 60) | bool | true; every minute starts a 60-second window, so the current time is always inside one | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdatetimeisinwindow%2Fexample-01.ncalc) |
| 2 | dateTimeIsInWindow('0 0 2 * * SUN', 7200, 'Europe/Amsterdam') | bool | true only between 02:00 and 04:00 on Sunday, Amsterdam time | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdatetimeisinwindow%2Fexample-02.ncalc) |
