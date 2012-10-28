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
using Afluistic.Tests.TestObjects;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class TypeExtensionsTests
    {
        private const string PropertyDescription = "reversed";
        private const string TypeDescription = "profile";

        [UIDescription(TypeDescription)]
        public class ObjectWithDescription
        {
            [UIDescription(PropertyDescription)]
            public string Name { get; set; }
        }

        public class When_asked_to_get_the_UI_description_for_a_Property
        {
            [TestFixture]
            public class Given_a_Property_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_name_of_the_Property()
                {
                    var description = TypeExtensions.GetUIDescription<TestObject>(x => x.Value);
                    description.ShouldBeEqualTo("Value");
                }
            }

            [TestFixture]
            public class Given_a_Property_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_description_from_the_attribute()
                {
                    var description = TypeExtensions.GetUIDescription<ObjectWithDescription>(x => x.Name);
                    description.ShouldBeEqualTo(PropertyDescription);
                }
            }
        }

        public class When_asked_to_get_the_UI_description_for_a_Type
        {
            [TestFixture]
            public class Given_a_Type_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_name_of_the_Type()
                {
                    var description = typeof(CommandHandler).GetUIDescription();
                    description.ShouldBeEqualTo(typeof(CommandHandler).Name);
                }
            }

            [TestFixture]
            public class Given_a_Type_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_description_from_the_attribute()
                {
                    var description = typeof(ObjectWithDescription).GetUIDescription();
                    description.ShouldBeEqualTo(TypeDescription);
                }
            }
        }

        public class When_asked_to_get_the_words_from_the_type_name
        {
            [TestFixture]
            public class Given_a_type_that_doesn_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_then_Type_name_split_into_words_at_capital_letters()
                {
                    var words = typeof(TypeExtensionsTests).GetTypeNameWords();
                    words.ShouldContainAllInOrder(new[] { "Type", "Extensions", "Tests" });
                }
            }

            [TestFixture]
            public class Given_a_type_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_then_Type_name_split_into_words_at_capital_letters()
                {
                    var words = typeof(ObjectWithDescription).GetTypeNameWords();
                    words.ShouldContainAllInOrder(new[] { "Object", "With", "Description" });
                }
            }
        }
    }
}