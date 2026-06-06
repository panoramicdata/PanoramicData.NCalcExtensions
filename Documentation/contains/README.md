# contains()

| Field | Value |
| --- | --- |
| Purpose | Determines whether one string contains another. |
| Parameters | * string searched-in text * string searched-for text |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | contains('haystack containing needle', 'needle') | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcontains%2Fexample-01.ncalc) |
| 2 | contains('haystack containing only hay', 'needle') | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcontains%2Fexample-02.ncalc) |

