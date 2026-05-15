```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
13th Gen Intel Core i9-13900HX, 1 CPU, 32 logical and 24 physical cores
.NET SDK 10.0.300
  [Host]   : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                                               | Mean        | Error       | StdDev     | Gen0   | Gen1   | Allocated |
|----------------------------------------------------- |------------:|------------:|-----------:|-------:|-------:|----------:|
| Construct_ExtendedExpression                         | 2,644.72 ns |   505.89 ns |  27.730 ns | 0.1945 |      - |    3712 B |
| Construct_And_Evaluate_ExtendedExpression            | 2,805.59 ns | 1,473.72 ns |  80.780 ns | 0.2174 |      - |    4120 B |
| Parse_Document_And_Evaluate_SimpleExtendedExpression | 2,824.91 ns | 2,659.87 ns | 145.796 ns | 0.1984 |      - |    3768 B |
| Evaluate_ExtendedExpression_Reused                   |    95.29 ns |   125.97 ns |   6.905 ns | 0.0216 |      - |     408 B |
| Evaluate_SimpleExtendedExpression_Reused             |    92.95 ns |    46.86 ns |   2.568 ns | 0.0216 |      - |     408 B |
| Evaluate_Where_Reused                                | 4,820.12 ns | 5,196.59 ns | 284.843 ns | 1.0376 | 0.0076 |   19512 B |
| Evaluate_RegexIsMatch_Reused                         |   169.77 ns |   140.16 ns |   7.683 ns | 0.0207 |      - |     392 B |
| Evaluate_GetProperty_JsonDocument_Reused             |   101.27 ns |    35.84 ns |   1.964 ns | 0.0237 |      - |     448 B |
