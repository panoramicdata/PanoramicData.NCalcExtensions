# Quick Start Guide for Session 2
## Phase 2: Push to 85% Coverage

**Current Status:** 81.0% line coverage, 73.7% branch coverage  
**Target:** 85% line coverage, 75% branch coverage  
**Estimated Time:** 4-5 hours  
**Date:** Ready for November 1, 2025

---

## ?? Start Here

### Step 1: Verify Current Status (5 minutes)

```powershell
# Navigate to project
cd C:\Users\david\Projects\PanoramicData.NCalcExtensions

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults

# Generate coverage report
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:"Html;TextSummary"

# View summary
Get-Content "CoverageReport\Summary.txt" | Select-Object -First 50

# Open HTML report
start CoverageReport/index.html
```

### Step 2: Priority Task List

Work through tasks in this order for maximum impact:

#### ?? Task 1: Max Function (1 hour, +0.5%)
**File:** `PanoramicData.NCalcExtensions.Test\MaxTests.cs`

**Tests to Add:**
- [ ] Max with unsigned types (uint, ulong, ushort)
- [ ] Max with signed byte types (sbyte)
- [ ] Max with very large numbers near type limits
- [ ] Max with empty collection and lambda (error case)
- [ ] Max with all null values in typed collection
- [ ] Max with lambda returning null
- [ ] Max with mixed comparable types

**Example Pattern:**
```csharp
[Fact]
public void Max_UnsignedInt_ReturnsMax()
{
    var expression = new ExtendedExpression("max(listOf('uint', 1, 2, 3))");
    expression.Evaluate().Should().Be(3u);
}
```

#### ?? Task 2: Min Function (1 hour, +0.5%)
**File:** `PanoramicData.NCalcExtensions.Test\MinTests.cs`

**Tests to Add:** (Same patterns as Max)
- [ ] Min with unsigned types
- [ ] Min with signed byte types
- [ ] Min with very large/small numbers
- [ ] Min with empty collection and lambda
- [ ] Min with all null values
- [ ] Min with lambda returning null
- [ ] Min with mixed comparable types

#### ?? Task 3: CountBy Function (45 minutes, +0.8%)
**File:** `PanoramicData.NCalcExtensions.Test\CountByTests.cs`

**Tests to Add:**
- [ ] CountBy with empty list
- [ ] CountBy with all same values
- [ ] CountBy with null grouping keys
- [ ] CountBy with lambda throwing exception
- [ ] CountBy with non-string grouping result (error case)
- [ ] CountBy with complex object grouping
- [ ] CountBy with special characters in keys

**Example Pattern:**
```csharp
[Fact]
public void CountBy_EmptyList_ReturnsEmptyDictionary()
{
    var expression = new ExtendedExpression("countBy(list(), 'n', 'n')");
    var result = expression.Evaluate();
    result.Should().BeOfType<Dictionary<string, int>>();
    ((Dictionary<string, int>)result).Should().BeEmpty();
}
```

#### ?? Task 4: ToString Function (30 minutes, +0.5%)
**File:** `PanoramicData.NCalcExtensions.Test\ToStringTests.cs`

**Tests to Add:**
- [ ] ToString with 'N' format (number with thousand separators)
- [ ] ToString with 'C' format (currency)
- [ ] ToString with 'P' format (percent)
- [ ] ToString with 'E' format (exponential)
- [ ] ToString with 'F' format (fixed-point)
- [ ] ToString with 'G' format (general)
- [ ] ToString with custom format string
- [ ] ToString with DateTime and culture-specific format
- [ ] ToString with invalid format (error case)

#### ?? Task 5: Humanize Function (30 minutes, +0.4%)
**File:** `PanoramicData.NCalcExtensions.Test\HumanizeTests.cs`

**Tests to Add:**
- [ ] Humanize with milliseconds
- [ ] Humanize with minutes
- [ ] Humanize with hours
- [ ] Humanize with days
- [ ] Humanize with weeks
- [ ] Humanize with years
- [ ] Humanize with 0 value
- [ ] Humanize with negative value (if supported)
- [ ] Humanize with invalid time unit (error case)
- [ ] Humanize with very large value

#### ?? Task 6: GetProperty Function (30 minutes, +0.3%)
**File:** `PanoramicData.NCalcExtensions.Test\GetPropertyTests.cs`

**Tests to Add:**
- [ ] GetProperty from JObject
- [ ] GetProperty from JsonDocument
- [ ] GetProperty from Dictionary<string, object>
- [ ] GetProperty from regular .NET object
- [ ] GetProperty with non-existent property (error case)
- [ ] GetProperty with null object (error case)
- [ ] GetProperty with nested property path
- [ ] GetProperty with case sensitivity

---

## ?? Session Checklist

### Before Starting
- [ ] Pull latest changes: `git pull`
- [ ] Verify clean working directory: `git status`
- [ ] Run existing tests: `dotnet test`
- [ ] Generate baseline coverage report

### During Work
- [ ] Follow test naming conventions: `FunctionName_Scenario_ExpectedResult`
- [ ] Use Theory for parameterized tests when appropriate
- [ ] Test both success and error paths
- [ ] Include edge cases (null, empty, boundary values)
- [ ] Run tests frequently: `dotnet test`
- [ ] Check coverage after each significant addition

### After Each Task
- [ ] Verify tests pass
- [ ] Check coverage improvement
- [ ] Commit with descriptive message
- [ ] Update COVERAGE_PLAN.md if needed

### End of Session
- [ ] Run full test suite: `dotnet test`
- [ ] Generate final coverage report
- [ ] Commit all changes
- [ ] Update COVERAGE_PLAN.md with actual progress
- [ ] Note next session priorities

---

## ?? Target Metrics for Session 2

| Metric | Start | Target | Expected Gain |
|--------|-------|--------|---------------|
| Line Coverage | 81.0% | 85%+ | +4%+ |
| Branch Coverage | 73.7% | 75%+ | +1.3%+ |
| Total Tests | 1090 | ~1180 | +90 |
| Max Coverage | 50.5% | 90%+ | +39.5% |
| Min Coverage | 50.5% | 90%+ | +39.5% |
| CountBy Coverage | 56.2% | 85%+ | +28.8% |
| ToString Coverage | 65.6% | 90%+ | +24.4% |

---

## ??? Useful Commands

### Coverage Generation
```powershell
# Full coverage with report
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:"Html;TextSummary"
start CoverageReport/index.html
```

### Quick Test Run
```powershell
# Run all tests
dotnet test

# Run specific test file
dotnet test --filter "FullyQualifiedName~MaxTests"

# Run specific test
dotnet test --filter "FullyQualifiedName~Max_UnsignedInt_ReturnsMax"
```

### Check Specific Function Coverage
```powershell
Get-Content "CoverageReport\Summary.txt" | Select-String -Pattern "Max\s|Min\s|CountBy|ToString|Humanize|GetProperty"
```

### Git Workflow
```powershell
# Check status
git status

# Add and commit
git add .
git commit -m "test: add comprehensive tests for Max/Min functions

Added branch coverage tests for Max and Min functions:
- All unsigned numeric types (uint, ulong, ushort)
- Signed byte types (sbyte)
- Boundary value testing
- Empty collection with lambda error cases
- Null value handling in all scenarios

Coverage Impact:
- Max: 50.5% ? 90%+ (+39.5%)
- Min: 50.5% ? 90%+ (+39.5%)
- Overall: 81.0% ? 82.0% (+1.0%)

Tests Added: +30
Total Tests: 1120"
```

---

## ?? Coverage Review Process

### After Each Major Addition
1. Generate coverage report
2. Open HTML report in browser
3. Navigate to the specific function
4. Check line-by-line coverage
5. Identify uncovered lines (red)
6. Identify partially covered branches (yellow)
7. Add tests to cover gaps
8. Re-run coverage
9. Verify improvement

### HTML Report Navigation
```
CoverageReport/index.html
  ?? PanoramicData.NCalcExtensions
      ?? Extensions
          ?? Max.cs (click to see line-by-line)
    ?? Red lines = uncovered
           ?? Yellow lines = partial branch coverage
       ?? Green lines = fully covered
```

---

## ?? Test Pattern Reference

### Basic Test
```csharp
[Fact]
public void FunctionName_Scenario_ExpectedBehavior()
{
    var expression = new ExtendedExpression("functionName(args)");
    expression.Evaluate().Should().Be(expectedValue);
}
```

### Parameterized Test
```csharp
[Theory]
[InlineData("input1", expectedOutput1)]
[InlineData("input2", expectedOutput2)]
public void FunctionName_MultipleScenarios(string input, object expected)
{
var expression = new ExtendedExpression($"functionName('{input}')");
    expression.Evaluate().Should().Be(expected);
}
```

### Error Test
```csharp
[Fact]
public void FunctionName_InvalidInput_ThrowsException()
{
    new ExtendedExpression("functionName(invalidArgs)")
        .Invoking(e => e.Evaluate())
        .Should()
        .Throw<FormatException>()
 .WithMessage("*expected error message pattern*");
}
```

### Using Variables
```csharp
[Fact]
public void FunctionName_WithVariable_Works()
{
var expression = new ExtendedExpression("functionName(myVar)");
    expression.Parameters["myVar"] = testValue;
    expression.Evaluate().Should().Be(expectedValue);
}
```

---

## ? Success Indicators

You're on track when:
- ? Tests are passing (99.5%+ pass rate)
- ? No compiler warnings introduced
- ? Coverage is increasing steadily
- ? Each function shows visible progress in HTML report
- ? Commit messages are descriptive
- ? Test names clearly describe scenarios

---

## ?? Troubleshooting

### Tests Failing
1. Check error message carefully
2. Verify function behavior in README.md
3. Test manually in a simple console app if needed
4. Check for null handling issues
5. Verify type conversions

### Coverage Not Increasing
1. Verify tests are actually running (check test count)
2. Check HTML report to see what lines are uncovered
3. Ensure branch conditions are tested (both true and false paths)
4. Look for early returns that might be skipped

### Compiler Warnings
1. Fix immediately - don't accumulate technical debt
2. Common issues: nullable reference types, unused variables
3. Use null-forgiving operator (!) when you've verified non-null

---

## ?? Quick Reference

**Project Location:** `C:\Users\david\Projects\PanoramicData.NCalcExtensions`  
**Test Project:** `PanoramicData.NCalcExtensions.Test`  
**Coverage Report:** `CoverageReport/index.html`  
**Documentation:** `COVERAGE_PLAN.md`, `SESSION_SUMMARY.md`

**Current Phase:** Phase 2 (81% ? 85%)  
**Next Phase:** Phase 3 (85% ? 90%)  
**End Goal:** Phase 5 (100%)

---

## ?? Session 2 Goal

**Start:** 81.0% line coverage  
**Target:** 85.0% line coverage  
**Stretch:** 86-87% if time permits

**Focus Functions:**
1. Max (critical)
2. Min (critical)
3. CountBy (high priority)
4. ToString (high priority)
5. Humanize (medium priority)
6. GetProperty (medium priority)

---

**Ready to start? Begin with Task 1: Max Function!** ??

**Good luck and maintain that 99.8% test pass rate!** ??
