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
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Commands.ArgumentChecks;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class InitTests
    {
        [TestFixture]
        public class When_asked_if_it_changes_the_application_settings
        {
            [Test]
            public void Should_return_true()
            {
                IoC.Get<Init>().ChangesTheApplicationSettings().ShouldBeTrue();
            }
        }

        [TestFixture]
        public class When_asked_if_it_changes_the_statement
        {
            [Test]
            public void Should_return_true()
            {
                IoC.Get<Init>().ChangesTheStatement().ShouldBeTrue();
            }
        }

        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_the_argument_is_not_a_valid_file_path : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("?")
                        .VerifyStandardErrorMatches(IsAFilePath.ErrorMessageText);
                }
            }

            [TestFixture]
            public class Given_too_few_arguments : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init()
                        .VerifyStandardErrorMatches(Init.FilePathNotSpecifiedMessageText);
                }
            }

            [TestFixture]
            public class Given_too_many_arguments : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("a", "b")
                        .VerifyStandardErrorMatches(Init.FilePathNotSpecifiedMessageText);
                }
            }

            [TestFixture]
            public class Given_valid_Execution_Arguments : IntegrationTestBase
            {
                private const string FilePath = @"x:\new.statement";

                [Test]
                public void Should_create_the_requested_file()
                {
                    FileExists(FilePath).ShouldBeTrue();
                }

                [Test]
                public void Should_not_write_an_error_message()
                {
                    StandardErrorText.Length.ShouldBeEqualTo(0);
                }

                [Test]
                public void Should_populate_account_types_with_the_default_values()
                {
                    Statement statement = Statement;
                    statement.AccountTypes.ShouldContainAll(Init.GetDefaultAccountTypes());
                }

                [Test]
                public void Should_populate_tax_reporting_categories_with_the_default_values()
                {
                    Statement statement = Statement;
                    statement.TaxReportingCategories.ShouldContainAll(Init.GetDefaultTaxReportingCategories());
                }

                [Test]
                public void Should_populate_transaction_types_with_the_default_values()
                {
                    Statement statement = Statement;
                    statement.TransactionTypes.ShouldContainAll(Init.GetDefaultTransactionTypes());
                }

                [Test]
                public void Should_update_the_settings_statement_path_to_the_new_filepath()
                {
                    var settingsResult = Settings;
                    settingsResult.HasErrors.ShouldBeFalse();
                    settingsResult.Item.StatementPath.ShouldBeEqualTo(FilePath);
                }

                [Test]
                public void Should_write_a_success_message()
                {
                    Regex.IsMatch(StandardOutText, Init.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    Subcutaneous.FromCommandline()
                        .Init(@"x:\previous.statement")
                        .ClearOutput()
                        .Init(FilePath);
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
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, Init.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}