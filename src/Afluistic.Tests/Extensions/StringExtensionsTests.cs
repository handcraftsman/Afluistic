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

using System.Linq;

using Afluistic.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class StringExtensionsTests
    {
        public class When_asked_to_convert_a_message_text_to_a_regex
        {
            [TestFixture]
            public class Given_a_message_text_containing_multiple_string_format_placeholders
            {
                [Test]
                public void Should_convert_the_placeholders_to_a_regex_match()
                {
                    const string messageText = "the {0} is {1}";
                    var regexText = messageText.MessageTextToRegex();
                    regexText.ShouldBeEqualTo("the .* is .*");
                }
            }

            [TestFixture]
            public class Given_a_message_text_containing_one_string_format_placeholder
            {
                [Test]
                public void Should_convert_the_placeholder_to_a_regex_match()
                {
                    const string messageText = "the {0} is round";
                    var regexText = messageText.MessageTextToRegex();
                    regexText.ShouldBeEqualTo("the .* is round");
                }
            }
        }

        public class When_asked_to_split_on_transition_to_capital_letter
        {
            [TestFixture]
            public class Given__AnotherUI
            {
                [Test]
                public void Should_return_the_words__Another_UI()
                {
                    const string input = "AnotherUI";
                    var result = input.SplitOnTransitionToCapitalLetter();
                    result.Length.ShouldBeEqualTo(2);
                    result.First().ShouldBeEqualTo("Another");
                    result.Last().ShouldBeEqualTo("UI");
                }
            }

            [TestFixture]
            public class Given__HelloWorld
            {
                [Test]
                public void Should_return_the_words__Hello_World()
                {
                    const string input = "HelloWorld";
                    var result = input.SplitOnTransitionToCapitalLetter();
                    result.Length.ShouldBeEqualTo(2);
                    result.First().ShouldBeEqualTo("Hello");
                    result.Last().ShouldBeEqualTo("World");
                }
            }

            [TestFixture]
            public class Given__Init
            {
                [Test]
                public void Should_return_the_word__Init()
                {
                    const string input = "Init";
                    var result = input.SplitOnTransitionToCapitalLetter();
                    result.Length.ShouldBeEqualTo(1);
                    result.First().ShouldBeEqualTo(input);
                }
            }
        }
    }
}