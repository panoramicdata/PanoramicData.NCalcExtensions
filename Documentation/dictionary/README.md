# dictionary()

| Field | Value |
| --- | --- |
| Purpose | Emits a Dictionary<string, object?>. |
| Parameters | * interlaced keys and values. You must provide an even number of parameters, and keys must evaluate to strings. |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | dictionary('KEY1', 'Hello', 'KEY2', 'Goodbye') | System.Collections.Generic.Dictionary<string, object?> | a dictionary containing 2 values with keys KEY1 and KEY2, and string values | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdictionary%2Fexample-01.ncalc) |
| 2 | dictionary('TRUE', true, 'FALSE', false) | System.Collections.Generic.Dictionary<string, object?> | a dictionary containing 2 values with keys TRUE and FALSE, and boolean values | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fdictionary%2Fexample-02.ncalc) |

