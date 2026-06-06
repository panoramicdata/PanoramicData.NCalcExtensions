# dateAdd()

| Field | Value |
| --- | --- |
| Purpose | Add a specified period to a DateTime. The following units are supported: * Years * Months * Days * Hours * Minutes * Seconds * Milliseconds |
| Parameters | * intialDateTime - A DateTime to which to add the period specified * quantity - The integer number of the units to be added * units - A string representing the units used to specify the period to be added |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | dateAdd(toDateTime('2019-03-05 05:09', 'yyyy-MM-dd HH:mm'), -90, 'days') |  | 90 days before (2018-12-05 05:09:00) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdateadd%2Fexample-01.ncalc) |
| 2 | dateAdd(toDateTime('2019-03-05 01:03:05', 'yyyy-MM-dd HH:mm:ss'), 2, 'hours') |  | 2 hours later (2019-03-05 03:03:05) | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdateadd%2Fexample-02.ncalc) |

