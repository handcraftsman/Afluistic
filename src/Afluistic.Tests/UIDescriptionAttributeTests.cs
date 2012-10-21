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

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests
{
    public class UIDescriptionAttributeTests
    {
        [TestFixture]
        public class Given_a_description_in_the_constructor
        {
            [Test]
            public void Should_be_able_to_get_the_description()
            {
                const string uiDescription = "test";
                var descriptor = new UIDescriptionAttribute(uiDescription);
                var description = descriptor.UIDescription;
                description.ShouldBeEqualTo(uiDescription);
            }
        }
    }
}