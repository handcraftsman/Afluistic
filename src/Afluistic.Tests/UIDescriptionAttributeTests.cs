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

using Afluistic.Tests.TestObjects;

using FluentAssert;

using NUnit.Framework;

using Afluistic.Extensions;

namespace Afluistic.Tests
{
    public class UIDescriptionAttributeTests
    {
        [TestFixture]
        public class Given_a_simple_description_in_the_constructor
        {
            [Test]
            public void Should_get_the_description()
            {
                const string uiDescription = "test";
                var descriptor = new UIDescriptionAttribute(uiDescription);
                var description = descriptor.UIDescription;
                description.ShouldBeEqualTo(uiDescription);
            }
        }

        [TestFixture]
        public class Given_a_description_with_a_type_reference_in_the_constructor
        {
            [Test]
            public void Should_get_the_description_with_type_reference_replaced_by_its_ui_description()
            {
                string uiDescription = "a $"+typeof(ObjectWithDescription).Name+" test";
                var descriptor = new UIDescriptionAttribute(uiDescription);
                var description = descriptor.UIDescription;
                description.ShouldBeEqualTo("a "+typeof(ObjectWithDescription).GetSingularUIDescription()+" test");
            }
        }
    }
}