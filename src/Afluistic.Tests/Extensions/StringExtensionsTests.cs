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

using System;
using System.IO;
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
            public class Given_a_message_text_containing_a_start_multiple_match_character_special_character
            {
                [Test]
                public void Should_escape_the_character_with_a_backslash()
                {
                    const string messageText = "the [0] is round";
                    var regexText = messageText.MessageTextToRegex();
                    regexText.ShouldBeEqualTo(@"the \[0] is round");
                }
            }

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

        public class When_asked_to_convert_a_path_to_a_statement_path
        {
            [TestFixture]
            public class Given_a_path_that_cannot_be_converted_to_an_absolute_path
            {
                [Test]
                public void Should_return_an_error_notification()
                {
                    const string path = "?";
                    var result = path.ToStatementPath();
                    result.HasErrors.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_a_path_that_resolves_to_a_directory
            {
                [Test]
                public void Should_return_a_path_created_by_combining_the_directory_with_the_default_file_name()
                {
                    const string path = @".";
                    var result = path.ToStatementPath();
                    result.HasErrors.ShouldBeFalse();
                    result.Item.ShouldBeEqualTo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Constants.DefaultStatementFileName)));
                }
            }

            [TestFixture]
            public class Given_a_path_that_resolves_to_a_file
            {
                [Test]
                public void Should_return_the_absolute_path_to_the_file()
                {
                    const string path = @".\Afluistic.pdb";
                    var result = path.ToStatementPath();
                    result.HasErrors.ShouldBeFalse();
                    result.Item.ShouldBeEqualTo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path)));
                }
            }
        }

        public class When_asked_to_convert_a_path_to_an_absolute_path
        {
            [TestFixture]
            public class Given_a_dot_dot_path
            {
                [Test]
                public void Should_return_the_current_directory_combined_with_the_input()
                {
                    const string path = @"..\foo";
                    var absolutePath = path.ToAbsolutePath();
                    absolutePath.Item.ShouldBeEqualTo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path)));
                }
            }

            [TestFixture]
            public class Given_a_path_containing_a_drive_designation
            {
                [Test]
                public void Should_return_the_input()
                {
                    const string path = @"c:\foo";
                    var absolutePath = path.ToAbsolutePath();
                    absolutePath.Item.ShouldBeEqualTo(path);
                }
            }

            [TestFixture]
            public class Given_dot
            {
                [Test]
                public void Should_return_the_current_directory()
                {
                    const string path = ".";
                    var absolutePath = path.ToAbsolutePath();
                    absolutePath.Item.ShouldBeEqualTo(Environment.CurrentDirectory);
                }
            }

            [TestFixture]
            public class Given_dot_slash_foo
            {
                [Test]
                public void Should_return_the_current_directory_combined_with_the_input()
                {
                    const string path = @".\foo";
                    var absolutePath = path.ToAbsolutePath();
                    absolutePath.Item.ShouldBeEqualTo(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path)));
                }
            }
        }

        public class When_asked_to_pluralize_a_string
        {
            [TestFixture]
            public class Given_an_input_that_does_not_end_with__s
            {
                [Test]
                public void Should_return_the_input_with_suffix__s()
                {
                    const string input = "cat";
                    var result = input.Pluralize();
                    result.ShouldBeEqualTo("cats");
                }
            }

            [TestFixture]
            public class Given_an_input_that_ends_with__s
            {
                [Test]
                public void Should_return_the_input_with_suffix__es()
                {
                    const string input = "boss";
                    var result = input.Pluralize();
                    result.ShouldBeEqualTo("bosses");
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