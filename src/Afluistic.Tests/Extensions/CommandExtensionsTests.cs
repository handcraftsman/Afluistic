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

using Afluistic.Commands;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class CommandExtensionsTests
    {
        public class SampleCommand : ICommand
        {
            public Notification Execute(ExecutionArguments executionArguments)
            {
                throw new NotImplementedException();
            }

            public void WriteUsage(TextWriter textWriter)
            {
                throw new NotImplementedException();
            }
        }

        public class When_asked_if_a_command_is_a_match_for_a_set_of_arguments
        {
            [TestFixture]
            public class Given_a_command_and_extra_arguments
            {
                [Test]
                public void Should_return_false_if_the_first_words_of_the_arguments_do_not_match_the_command_words_at_all()
                {
                    var result = new SampleCommand().IsMatch(new[] { "hello", "world", "woot" });
                    result.ShouldBeFalse();
                }

                [Test]
                public void Should_return_false_if_the_first_words_of_the_arguments_match_the_command_words_except_for_case()
                {
                    var result = new SampleCommand().IsMatch(new[] { "Sample", "Command", "Woot!" });
                    result.ShouldBeFalse();
                }

                [Test]
                public void Should_return_true_if_the_first_words_of_the_arguments_exactly_match_the_command_words()
                {
                    var result = new SampleCommand().IsMatch(new[] { "sample", "command", "woot" });
                    result.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_a_command_and_the_correct_number_of_arguments
            {
                [Test]
                public void Should_return_false_if_the_arguments_do_not_match_the_command_words_at_all()
                {
                    var result = new SampleCommand().IsMatch(new[] { "hello", "world" });
                    result.ShouldBeFalse();
                }

                [Test]
                public void Should_return_false_if_the_arguments_match_the_command_words_except_for_case()
                {
                    var result = new SampleCommand().IsMatch(new[] { "Sample", "Command" });
                    result.ShouldBeFalse();
                }

                [Test]
                public void Should_return_true_if_the_arguments_exactly_match_the_command_words()
                {
                    var result = new SampleCommand().IsMatch(new[] { "sample", "command" });
                    result.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_a_command_but_insufficient_arguments
            {
                [Test]
                public void Should_return_false()
                {
                    var result = new SampleCommand().IsMatch(new[] { "foo" });
                    result.ShouldBeFalse();
                }
            }
        }

        public class When_asked_to_get_command_words
        {
            [TestFixture]
            public class Given_a_Type_with_multiple_words_in_its_name
            {
                [Test]
                public void Should_lowercase_and_return_the_words_from_the_type_name()
                {
                    var words = new SampleCommand().GetCommandWords();
                    words.ShouldContainAllInOrder(new[] { "sample", "command" });
                }
            }

            [TestFixture]
            public class Given_a_Type_with_one_word_in_its_name
            {
                [Test]
                public void Should_lowercase_and_return_the_word_from_the_type_name()
                {
                    var words = new Sample().GetCommandWords();
                    words.ShouldContainAllInOrder(new[] { "sample" });
                }

                public class Sample : ICommand
                {
                    public Notification Execute(ExecutionArguments executionArguments)
                    {
                        throw new NotImplementedException();
                    }

                    public void WriteUsage(TextWriter textWriter)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }
    }
}