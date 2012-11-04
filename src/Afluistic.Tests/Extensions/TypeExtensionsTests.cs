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
        public class When_asked_to_get_the_plural_UI_description_for_a_Property
        {
            [TestFixture]
            public class Given_a_Property_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_pluralized_name_of_the_Property()
                {
                    var description = TypeExtensions.GetPluralUIDescription<TestObject>(x => x.Value);
                    description.ShouldBeEqualTo("Values");
                }
            }

            [TestFixture]
            public class Given_a_Property_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_plural_description_from_the_attribute()
                {
                    var description = TypeExtensions.GetPluralUIDescription<ObjectWithDescription>(x => x.Name);
                    description.ShouldBeEqualTo(ObjectWithDescription.PluralPropertyDescription);
                }
            }
        }

        public class When_asked_to_get_the_plural_UI_description_for_a_Type
        {
            [TestFixture]
            public class Given_a_Type_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_words_in_the_name_of_the_Type_with_the_final_word_pluralized()
                {
                    var description = typeof(CommandHandler).GetPluralUIDescription();
                    description.ShouldBeEqualTo("Command Handlers");
                }
            }

            [TestFixture]
            public class Given_a_Type_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_plural_description_from_the_attribute()
                {
                    var description = typeof(ObjectWithDescription).GetPluralUIDescription();
                    description.ShouldBeEqualTo(ObjectWithDescription.PluralTypeDescription);
                }
            }
        }

        public class When_asked_to_get_the_singular_UI_description_for_a_Property
        {
            [TestFixture]
            public class Given_a_Property_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_name_of_the_Property()
                {
                    var description = TypeExtensions.GetSingularUIDescription<TestObject>(x => x.Value);
                    description.ShouldBeEqualTo("Value");
                }
            }

            [TestFixture]
            public class Given_a_Property_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_singular_description_from_the_attribute()
                {
                    var description = TypeExtensions.GetSingularUIDescription<ObjectWithDescription>(x => x.Name);
                    description.ShouldBeEqualTo(ObjectWithDescription.PropertyDescription);
                }
            }
        }

        public class When_asked_to_get_the_singular_UI_description_for_a_Type
        {
            [TestFixture]
            public class Given_a_Type_that_does_not_have_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_words_in_the_name_of_the_Type()
                {
                    var description = typeof(CommandHandler).GetSingularUIDescription();
                    description.ShouldBeEqualTo(typeof(CommandHandler).GetTypeNameWordsAsString());
                }
            }

            [TestFixture]
            public class Given_a_Type_that_has_a_UIDescription_attribute
            {
                [Test]
                public void Should_return_the_singular_description_from_the_attribute()
                {
                    var description = typeof(ObjectWithDescription).GetSingularUIDescription();
                    description.ShouldBeEqualTo(ObjectWithDescription.TypeDescription);
                }
            }
        }

        public class When_asked_to_get_the_words_from_the_Type_name
        {
            [TestFixture]
            public class Given_a_Type
            {
                [Test]
                public void Should_return_the_Type_name_split_into_words_at_capital_letters()
                {
                    var words = typeof(TypeExtensionsTests).GetTypeNameWords();
                    words.ShouldContainAllInOrder(new[] { "Type", "Extensions", "Tests" });
                }
            }
        }

        public class When_asked_to_get_the_words_from_the_Type_name_as_a_string
        {
            [TestFixture]
            public class Given_a_Type
            {
                [Test]
                public void Should_return_the_Type_name_split_into_words_at_capital_letters_and_separated_by_spaces()
                {
                    var words = typeof(TypeExtensionsTests).GetTypeNameWordsAsString();
                    words.ShouldBeEqualTo("Type Extensions Tests");
                }
            }
        }
    }
}