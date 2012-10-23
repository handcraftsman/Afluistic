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
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class ShowSettingsTests
    {
        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_no_parameters_and_the_application_settings_have_been_initialized : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_not_return_errors_or_warnings()
                {
                    _result.HasErrors.ShouldBeFalse();
                    _result.HasWarnings.ShouldBeFalse();
                }

                [Test]
                public void Should_write_to_the_standard_output()
                {
                    StandardOutText.Length.ShouldNotBeEqualTo(0);
                }

                protected override void Before_first_test()
                {
                    IoC.Get<IApplicationSettingsService>().Save(new ApplicationSettings
                        {
                            StatementPath = @"x:\previous.statement"
                        });
                    var command = IoC.Get<ShowSettings>();
                    _result = command.Execute(command.GetCommandWords());
                }
            }

            [TestFixture]
            public class Given_no_parameters_and_the_application_settings_have_not_been_initialized : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_not_write_to_the_standard_output()
                {
                    StandardErrorText.ShouldBeEqualTo("");
                }

                [Test]
                public void Should_return_an_error_about_the_missing_settings_file()
                {
                    _result.HasWarnings.ShouldBeTrue();
                    Regex.IsMatch(_result.Warnings, ApplicationSettingsService.MissingSettingsFileMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<ShowSettings>();
                    _result = command.Execute(command.GetCommandWords());
                }
            }

            [TestFixture]
            public class Given_too_many_parameters : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_return_an_error_message_for_too_many_parameters()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, ShowSettings.TooManyArgumentsMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<ShowSettings>();
                    _result = command.Execute(command.GetCommandWords().Concat(new[] { "?" }).ToArray());
                }
            }
        }

        public class When_asked_to_write_its_usage_information
        {
            [TestFixture]
            public class Given_a_TextWriter : IntegrationTestBase
            {
                [Test]
                public void Should_write_its_usage_information_to_the_TextWriter()
                {
                    var writer = new StringWriter();
                    var command = IoC.Get<ShowSettings>();
                    command.WriteUsage(writer);
                    writer.ToString().ShouldContain(String.Join(" ", command.GetCommandWords()));
                }
            }
        }
    }
}