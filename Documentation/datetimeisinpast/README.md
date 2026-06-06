# dateTimeIsInPast()

| Field | Value |
| --- | --- |
| Purpose | Returns whether a date and time is in the past, with optional timezone correction. |
| Parameters | * The date and time under test * optionally, the name of the timezone that the date and time represents |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | dateTimeIsInPast(toDateTime('2001-01-01T00:00:00', 'YYYY-MM-ddTHH:mm:ss')) | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdatetimeisinpast%2Fexample-01.ncalc) |
| 2 | dateTimeIsInPast(toDateTime('2201-01-01T00:00:00', 'YYYY-MM-ddTHH:mm:ss')) | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdatetimeisinpast%2Fexample-02.ncalc) |
| 3 | dateTimeIsInPast(now(), 'Africa/Luanda') |  | true; UTC is always behind West Africa Time | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdatetimeisinpast%2Fexample-03.ncalc) |

