# jPath()

| Field | Value |
| --- | --- |
| Purpose | Selects a single value from a JObject using a [JPath](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) expression |
| Parameters | * input JObject * JPath string expression |
| Examples | 6 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | jPath(sourceJObject, 'name') | string | 'bob' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-01.ncalc) |
| 2 | jPath(sourceJObject, 'details.[\'this.thing\']') | string | 'woo' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-02.ncalc) |
| 3 | jPath(sourceJObject, 'size') |  | an exception is thrown | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-03.ncalc) |
| 4 | jPath(sourceJObject, 'size', True) |  | null is returned | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-04.ncalc) |
| 5 | jPath(sourceJObject, 'numbers[0]') | int | 1 | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-05.ncalc) |
| 6 | jPath(sourceJObject, 'arrayList[?(@key==\\'key1\\')]') |  | "value1" | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjpath%2Fexample-06.ncalc) |

