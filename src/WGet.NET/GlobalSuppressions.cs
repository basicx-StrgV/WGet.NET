// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "It looks like this error is not working right in the newes version of sonarlint. The parameters are used in the method.", Scope = "namespaceanddescendants", Target = "~N:WGetNET")]
