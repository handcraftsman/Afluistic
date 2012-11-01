// * **************************************************************************
// * Copyright (c) Clinton Sheppard <sheppard@cs.unm.edu>
// *
// * This source code is subject to terms and conditions of the MIT License.
// * A copy of the license can be found in the License.txt file
// * at the root of this distribution.
// * By using this source code in any fashion, you are agreeing to be bound by
// * the terms of the MIT License.
// * You must not remove this notice from this software.
// *
// * source repository: https://github.com/handcraftsman/Afluistic
// * **************************************************************************
namespace Afluistic.Tests.TestObjects
{
    [UIDescription(TypeDescription, PluralTypeDescription)]
    public class ObjectWithDescription
    {
        public const string PluralPropertyDescription = "horses";
        public const string PluralTypeDescription = "foxes";
        public const string PropertyDescription = "cat";
        public const string TypeDescription = "dog";
        [UIDescription(PropertyDescription, PluralPropertyDescription)]
        public string Name { get; set; }
    }
}