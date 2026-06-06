# sanitize()

| Field | Value |
| --- | --- |
| Purpose | Sanitize a string, replacing any characters outside of the allowed set. |
| Parameters | * input - the string to be sanitized * allowedCharacters - all of the characters that are allowed * replacementCharacters (optional) - the characters to insert in place of any that are not allowed (defaults : empty string) |
| Examples | 6 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | sanitize('ab cd', 'abcdefghi') | string | 'abcd' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-01.ncalc) |
| 2 | sanitize('ab cd./', 'abcdefghi.') | string | 'abcd.' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-02.ncalc) |
| 3 | sanitize('ab cd./', 'CBa') | string | 'a' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-03.ncalc) |
| 4 | sanitize('ab cd', 'abCD', '?') | string | 'ab???' | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-04.ncalc) |
| 5 | sanitize('ab cd', 'ab', '?') | string | 'ab???' | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-05.ncalc) |
| 6 | sanitize('ab cd', 'ab', '-') | string | 'ab---' | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsanitize%2Fexample-06.ncalc) |

