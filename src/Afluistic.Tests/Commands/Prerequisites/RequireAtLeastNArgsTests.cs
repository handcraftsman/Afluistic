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

using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Commands.Prerequisites;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.Prerequisites
{
    public class RequireAtLeastNArgsTests
    {
        public class When_asked_to_Check
        {
            [TestFixture]
            public class Given_the_prerequisite_fails_due_to_too_few_arguments_and_has_custom_message_text
            {
                public const string TooFewArgumentsCustomMessageText = "Say this if too few";
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new string[] { }
                        };
                    _result = new RequireAtLeastNArgs(1, TooFewArgumentsCustomMessageText)
                        .Check(executionArguments);
                }

                [Test]
                public void Error_notification_should_use_the_custom_error_text()
                {
                    Regex.IsMatch(_result.Errors, TooFewArgumentsCustomMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_the_prerequisite_fails_due_to_too_few_arguments_and_has_default_message_text
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new string[] { }
                        };
                    _result = new RequireAtLeastNArgs(1)
                        .Check(executionArguments);
                }

                [Test]
                public void Error_notification_text_should_use_the_default_message_text()
                {
                    Regex.IsMatch(_result.Errors, RequireAtLeastNArgs.TooFewArgumentsMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_the_prerequisite_has_more_than_the_minimum_number_of_required_arguments
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { "a", "b", "c" }
                        };
                    _result = new RequireAtLeastNArgs(2)
                        .Check(executionArguments);
                }

                [Test]
                public void Should_not_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_the_prerequisite_has_the_minimum_number_of_arguments
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { "a" }
                        };
                    _result = new RequireAtLeastNArgs(1)
                        .Check(executionArguments);
                }

                [Test]
                public void Should_not_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }
            }
        }
    }
}