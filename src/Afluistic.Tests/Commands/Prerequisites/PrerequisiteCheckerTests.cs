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
using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;
using Afluistic.Tests.TestObjects.Commands;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.Prerequisites
{
    public class PrerequisiteCheckerTests
    {
        public class When_asked_to_Check
        {
            public class Given_a_command_with_multiple_prerequisites
            {
                [TestFixture]
                public class Given_all_prerequisites_pass
                {
                    private Notification _result;

                    [TestFixtureSetUp]
                    public void Before_first_test()
                    {
                        var executionArguments = new ExecutionArguments
                            {
                                Args = new[] { "a" },
                                ApplicationSettings = new Notification<ApplicationSettings>()
                            };
                        _result = new PrerequisiteChecker()
                            .Check(new CommandWithMultiplePrerequisites(), executionArguments);
                    }

                    [Test]
                    public void Should_not_return_an_error_notification()
                    {
                        _result.HasErrors.ShouldBeFalse();
                    }
                }

                [TestFixture]
                public class Given_the_first_prerequisite_fails
                {
                    private Notification _result;

                    [TestFixtureSetUp]
                    public void Before_first_test()
                    {
                        var executionArguments = new ExecutionArguments
                            {
                                Args = new[] { "a", "b", "c" }
                            };
                        _result = new PrerequisiteChecker()
                            .Check(new CommandWithMultiplePrerequisites(), executionArguments);
                    }

                    [Test]
                    public void Should_return_the_error_error_notification_from_the_first_prerequisite()
                    {
                        Regex.IsMatch(_result.Errors, RequireAdditionalArgs.TooManyArgumentsMessageText.MessageTextToRegex()).ShouldBeTrue();
                    }
                }

                [TestFixture]
                public class Given_the_first_prerequisite_passes_and_the_second_prerequisite_fails
                {
                    private Notification _result;

                    [TestFixtureSetUp]
                    public void Before_first_test()
                    {
                        var executionArguments = new ExecutionArguments
                            {
                                Args = new[] { "a" },
                                ApplicationSettings = Notification.ErrorFor("pretend").ToNotification<ApplicationSettings>()
                            };
                        _result = new PrerequisiteChecker()
                            .Check(new CommandWithMultiplePrerequisites(), executionArguments);
                    }

                    [Test]
                    public void Should_return_the_error_notification_from_the_second_prerequisite()
                    {
                        _result.Errors.ShouldBeEqualTo("pretend");
                    }
                }
            }

            [TestFixture]
            public class Given_a_command_with_no_prerequisites
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { "a" }
                        };
                    _result = new PrerequisiteChecker()
                        .Check(new CommandWithNoPrerequisites(), executionArguments);
                }

                [Test]
                public void Should_not_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }
            }

            public class Given_a_command_with_one_prerequisite
            {
                [TestFixture]
                public class Given_the_prerequisite_fails
                {
                    private Notification _result;

                    [TestFixtureSetUp]
                    public void Before_first_test()
                    {
                        var executionArguments = new ExecutionArguments
                            {
                                Args = new[] { "a", "b", "c" }
                            };
                        _result = new PrerequisiteChecker()
                            .Check(new CommandWithOnePrerequisite(), executionArguments);
                    }

                    [Test]
                    public void Should_return_the_error_notification_text_from_the_failed_prerequisite()
                    {
                        Regex.IsMatch(_result.Errors, RequireAdditionalArgs.TooManyArgumentsMessageText.MessageTextToRegex()).ShouldBeTrue();
                    }
                }

                [TestFixture]
                public class Given_the_prerequisite_passes
                {
                    private Notification _result;

                    [TestFixtureSetUp]
                    public void Before_first_test()
                    {
                        var executionArguments = new ExecutionArguments
                            {
                                Args = new[] { "a" }
                            };
                        _result = new PrerequisiteChecker()
                            .Check(new CommandWithOnePrerequisite(), executionArguments);
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
}