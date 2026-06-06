# nullCoalesce()

| Field | Value |
| --- | --- |
| Purpose | Returns the first parameter that is not null, otherwise: null. |
| Parameters | * any number of objects |
| Examples | 5 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | nullCoalesce() | object | null | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fnullcoalesce%2Fexample-01.ncalc) |
| 2 | nullCoalesce(1, null) | int | 1 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fnullcoalesce%2Fexample-02.ncalc) |
| 3 | nullCoalesce(null, 1, 2, 3) | int | 1 | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fnullcoalesce%2Fexample-03.ncalc) |
| 4 | nullCoalesce(null, null, null) | object | null | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fnullcoalesce%2Fexample-04.ncalc) |
| 5 | nullCoalesce(null, null, 'xxx', 3) | string | 'xxx' | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fnullcoalesce%2Fexample-05.ncalc) |

