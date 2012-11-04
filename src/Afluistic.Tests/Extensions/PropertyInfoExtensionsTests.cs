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

using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Tests.TestObjects;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class PropertyInfoExtensionsTests
    {
        public class When_asked_to_get_the_words_from_a_Property_name_as_a_string
        {
            [TestFixture]
            public class Given_a_Property
            {
                [Test]
                public void Should_return_the_Property_name_split_into_words_at_capital_letters_and_separated_by_spaces()
                {
                    var property = typeof(TestObject).GetProperty(Reflection.GetFinalPropertyName((TestObject t) => t.MultiWordPropertyName));
                    var words = property.GetPropertyNameWordsAsString();
                    words.ShouldBeEqualTo("Multi Word Property Name");
                }
            }
        }
    }
}