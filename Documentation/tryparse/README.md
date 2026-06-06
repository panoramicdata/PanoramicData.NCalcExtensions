# tryParse()

| Field | Value |
| --- | --- |
| Purpose | Returns a boolean result of an attempted cast. |
| Parameters | * type * value * key - for use with the retrieve() function |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | tryParse('int', '1', 'outputVariable') | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftryparse%2Fexample-01.ncalc) |
| 2 | tryParse('int', 'string', 'outputVariable') | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftryparse%2Fexample-02.ncalc) |

