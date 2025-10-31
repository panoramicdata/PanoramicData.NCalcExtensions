# 100% Code Coverage Plan
## PanoramicData.NCalcExtensions

**Target:** 100% Line Coverage, 100% Branch Coverage  
**Current:** 80.3% Line Coverage, 73.2% Branch Coverage  
**Timeline:** 2-3 weeks (accelerated)  
**Status:** ?? In Progress

---

## ?? Coverage Milestones

| Milestone | Line Coverage | Branch Coverage | Target Date | Status |
|-----------|---------------|-----------------|-------------|--------|
| **Phase 0** (Baseline) | 78.2% | 71.7% | Oct 31, 2025 | ? Complete |
| **Phase 1** (Critical) | **100%** | **100%** | Nov 7, 2025 | ?? In Progress (80.3%) |
| **Phase 2** (Polish) | **100%** | **100%** | Nov 14, 2025 | ? Pending |
| **Phase 3** (Maintain) | **100%** | **100%** | Ongoing | ? Pending |

---

## ?? Updated Strategy: Aggressive 100% Coverage Push

### Current Status (as of latest commit)
- **Line Coverage:** 80.3% (was 78.2%, +2.1%)
- **Branch Coverage:** 73.2% (was 71.7%, +1.5%)
- **Tests Added:** 70 new tests
- **Gap to 100%:** 19.7% line coverage, 26.8% branch coverage

### Immediate Focus: Complete Phase 1 to 100%

**Estimated Time:** 4-6 hours  
**Target:** 100% line coverage, 100% branch coverage

---

## ?? Phase 1: Complete Coverage (Revised)

### Goal: Achieve 100% Line Coverage, 100% Branch Coverage

### Completed Tasks ?
- ? ListHelpers: 0% ? 100%
- ? LastIndexOf: 0% ? 100%
- ? TypeHelper: 0% ? 100%
- ? Sum: 59% ? 67.2%
- ? If: 62.9% ? 85.1%
- ? DateTimeIsInPast: 63.6% ? ~70%
- ? DateTimeIsInFuture: 63.6% ? ~70%

### Remaining High-Impact Tasks ??

#### Task 1.8: Parameters Function (16% ? 100%)
**Priority:** CRITICAL - Lowest coverage  
**Estimated Time:** 1.5 hours  
**Coverage Impact:** +2-3%

```csharp
// Add tests for:
- Getting all parameters
- Getting specific parameter by name
- Non-existent parameters
- Null parameter handling
- Empty parameter dictionary
- Complex parameter types
```

#### Task 1.9: CountBy Function (56.2% ? 100%)
**Priority:** HIGH  
**Estimated Time:** 1 hour  
**Coverage Impact:** +1-1.5%

```csharp
// Add tests for:
- Grouping by various property types
- Empty lists
- Null values in groups
- Complex object grouping
- Lambda expressions
```

#### Task 1.10: Max/Min Branch Coverage (50.5% ? 100%)
**Priority:** HIGH  
**Estimated Time:** 45 minutes  
**Coverage Impact:** +1%

```csharp
// Add tests for:
- All numeric type branches
- String comparisons edge cases
- Null handling in all code paths
- Type conversion scenarios
```

#### Task 1.11: IsNaN Function (61.1% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- Valid NaN values
- Non-NaN numbers
- Infinity values
- Null inputs
- String conversions
```

#### Task 1.12: In Function (61.5% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- Value in list
- Value not in list
- Null value handling
- Different data types
- Empty lists
```

#### Task 1.13: Substring Edge Cases (65% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- Negative indices
- Index beyond length
- Zero length
- Very large length
- Unicode characters
```

#### Task 1.14: ToString Variations (65.6% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- All format specifiers
- Culture-specific formats
- Null values
- Invalid format strings
- Type conversion errors
```

#### Task 1.15: Humanize Time Units (65.7% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- All time unit types
- Edge case values (0, negative, max)
- Invalid time units
- Overflow scenarios
```

#### Task 1.16: Remaining Functions (65-80% ? 100%)
**Priority:** MEDIUM  
**Estimated Time:** 2 hours  
**Coverage Impact:** +3-5%

Focus on:
- IsSet (66.6%)
- ToLower/ToUpper (70%)
- Trim (70%)
- TimeSpan (72.9%)
- All other <80% functions

---

## ?? Execution Plan for 100% Coverage

### Session 1: Critical Functions (2 hours)
1. Parameters function - comprehensive tests
2. CountBy scenarios - all grouping cases
3. Max/Min - complete branch coverage

**Expected:** 80.3% ? 85%

### Session 2: Medium Priority (2 hours)  
4. IsNaN, In, Substring - edge cases
5. ToString, Humanize - format variations
6. IsSet, String functions - remaining branches

**Expected:** 85% ? 92%

### Session 3: Final Push (2 hours)
7. Review coverage report line-by-line
8. Target every uncovered line
9. Add integration tests for complex scenarios
10. Achieve 100% coverage

**Expected:** 92% ? 100%

---

## ?? Success Criteria (Updated)

### Phase 1 Complete When:
? Line Coverage: **100%** (not 85%)  
? Branch Coverage: **100%** (not 75%)  
? Tests Passing: **100%** (excluding 2 known NCalc bugs)  
? Zero Warnings  
? All code paths tested

### No More "Good Enough"
- Every line must be covered
- Every branch must be tested
- Every edge case must be validated
- Every error path must be verified

---

## ??? Coverage Enforcement (Already Implemented)

### Publish Script
- ? Requires 90% minimum (can be adjusted to 100%)
- ? Blocks publish if below threshold
- ? Force override available for emergencies

### Recommended Update:
```powershell
.\Publish.ps1 -MinCoverage 100  # Set to 100% requirement
```

---

## ?? Progress Tracking (Updated)

### Phase 1 Progress (Targeting 100%)
- ? ListHelpers: 0% ? 100%
- ? LastIndexOf: 0% ? 100%
- ? TypeHelper: 0% ? 100%
- ? Parameters: 16% ? 100% (NEXT)
- ? CountBy: 56.2% ? 100%
- ? Max: 50.5% ? 100%
- ? Min: 50.5% ? 100%
- ? Sum: 67.2% ? 100%
- ? If: 85.1% ? 100% (nearly complete)
- ? ALL OTHERS ? 100%

**Target:** 100% line coverage, 100% branch coverage  
**Current:** 80.3% / 100% (19.7% remaining)

---

## ?? Blocking Issues (Updated)

### Publish Will Be Blocked If:
1. Line coverage < **100%** (target updated from 90%)
2. Branch coverage < **100%** (target updated from 85%)
3. Tests failing > 2 (excluding known NCalc bugs)
4. New warnings introduced

### Zero Tolerance for Uncovered Code
- No "acceptable" coverage gaps
- Every function must have complete tests
- Every edge case must be covered
- Every error must be validated

---

**Plan Updated:** October 31, 2025  
**New Target:** 100% Coverage (all phases)  
**Commitment:** No code ships without tests  
**Status:** ?? Actively Working Towards 100%
