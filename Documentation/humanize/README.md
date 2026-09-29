# humanize()

| Field | Value |
| --- | --- |
| Purpose | Humanizes the value text. |
| Parameters | * value * timeUnit * optionally, resolution: the time unit to round the output to. The value is rounded half away from zero to a whole number of that unit, and nothing finer is shown. 'weeks' and 'years' give a single count such as '1 week'. When omitted, the output is not rounded and runs to whole seconds. |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | humanize(3600, 'seconds') | string | '1 hour' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fhumanize%2Fexample-01.ncalc) |
| 2 | humanize(0.51428, 'weeks', 'hours') | string | '3 days 14 hours' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fhumanize%2Fexample-02.ncalc) |
| 3 | humanize(0.51428, 'weeks', 'days') | string | '4 days' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fhumanize%2Fexample-03.ncalc) |
