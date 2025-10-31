﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: SuppressMessage(
	 "Globalization",
	 "CA1303:Do not pass literals as localized parameters",
	 Justification = "The library will not be localized",
	 Scope = "namespaceanddescendants",
	 Target = "~N:PanoramicData.NCalcExtensions.Test"
)]

[assembly: SuppressMessage(
	"Performance",
	"CA1861:Avoid constant arrays as arguments",
	Justification = "Convenience for Unit Tests",
	 Scope = "namespaceanddescendants",
	 Target = "~N:PanoramicData.NCalcExtensions.Test"
)]

[assembly: SuppressMessage(
	"Design",
	"CA1515:Consider making public types internal",
	Justification = "Test base classes need to be public for inheritance",
	Scope = "type",
	Target = "~T:PanoramicData.NCalcExtensions.Test.NCalcTest"
)]
