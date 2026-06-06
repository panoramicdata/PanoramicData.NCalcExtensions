# regexIsMatch()

| Field | Value |
| --- | --- |
| Purpose | Determine whether a string matches a regex |
| Parameters | * input * regex |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | regexIsMatch('abcdef', '^ab.+') | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexismatch%2Fexample-01.ncalc) |
| 2 | regexIsMatch('Zbcdef', '^ab.+') | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexismatch%2Fexample-02.ncalc) |

