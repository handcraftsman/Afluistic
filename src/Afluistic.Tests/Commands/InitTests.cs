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

using StringExtensions = Afluistic.Extensions.StringExtensions;

namespace Afluistic.Tests.Commands
{
    public class InitTests
    {
        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_a_valid_filepath_and_the_application_settings_have_been_initialized : IntegrationTestBase
            {
                private const string FilePath = @"x:\test";
                private Notification _result;

                [Test]
                public void Should_create_the_requested_file()
                {
                    FileExists(FilePath).ShouldBeTrue();
                }

                [Test]
                public void Should_not_write_to_output()
                {
                    StandardErrorText.ShouldBeEqualTo("");
                    StandardOutText.ShouldBeEqualTo("");
                }

                [Test]
                public void Should_return_a_success_message()
                {
                    _result.HasErrors.ShouldBeFalse();
                    _result.HasWarnings.ShouldBeFalse();
                    Regex.IsMatch(_result.Infos, Init.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                [Test]
                public void Should_update_the_settings_statement_path_to_the_new_filepath()
                {
                    var settingsResult = Settings;
                    settingsResult.HasErrors.ShouldBeFalse();
                    settingsResult.Item.StatementPath.ShouldBeEqualTo(FilePath);
                }

                protected override void Before_first_test()
                {
                    IoC.Get<IApplicationSettingsService>().Save(new ApplicationSettings
                        {
                            StatementPath = @"x:\previous.statement"
                        });
                    var command = IoC.Get<Init>();
                    _result = command.Execute(command.GetCommandWords().Concat(new[] { FilePath }).ToArray());
                }
            }

            [TestFixture]
            public class Given_a_valid_filepath_and_the_application_settings_have_not_been_initialized : IntegrationTestBase
            {
                private const string FilePath = @"x:\test";
                private Notification _result;

                [Test]
                public void Should_create_the_requested_file()
                {
                    FileExists(FilePath).ShouldBeTrue();
                }

                [Test]
                public void Should_return_a_success_message()
                {
                    _result.HasErrors.ShouldBeFalse();
                    _result.HasWarnings.ShouldBeFalse();
                    _result.Infos.Length.ShouldBeGreaterThan(0);
                }

                [Test]
                public void Should_update_the_settings_statement_path_to_contain_the_filepath()
                {
                    var settingsResult = Settings;
                    settingsResult.HasErrors.ShouldBeFalse();
                    settingsResult.Item.StatementPath.ShouldBeEqualTo(FilePath);
                }

                [Test]
                public void Should_write_a_warning_about_the_missing_settings_file_to_the_standard_output()
                {
                    Regex.IsMatch(StandardOutText, ApplicationSettingsService.MissingSettingsFileMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<Init>();
                    _result = command.Execute(command.GetCommandWords().Concat(new[] { FilePath }).ToArray());
                }
            }

            [TestFixture]
            public class Given_an_invalid_filepath_and_the_application_settings_have_been_initialized : IntegrationTestBase
            {
                private const string FilePath = @"?";
                private Notification _result;

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, StringExtensions.ErrorConvertingToAbsolutePathMesssageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    IoC.Get<IApplicationSettingsService>().Save(new ApplicationSettings
                        {
                            StatementPath = @"x:\previous.statement"
                        });
                    var command = IoC.Get<Init>();
                    _result = command.Execute(command.GetCommandWords().Concat(new[] { FilePath }).ToArray());
                }
            }

            [TestFixture]
            public class Given_parameters_with_a_filepath : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_return_an_error_message_for_the_missing_file_path()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, Init.FilePathNotSpecifiedMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<Init>();
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
                    Regex.IsMatch(_result.Errors, Init.TooManyArgumentsMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<Init>();
                    _result = command.Execute(command.GetCommandWords().Concat(new[] { "a", "b", "c" }).ToArray());
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
                    var command = IoC.Get<Init>();
                    command.WriteUsage(writer);
                    writer.ToString().ShouldContain(String.Join(" ", command.GetCommandWords()));
                }
            }
        }
    }
}