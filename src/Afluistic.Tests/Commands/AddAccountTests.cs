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
using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class AddAccountTests
    {
        [TestFixture]
        public class When_asked_if_it_changes_the_application_settings
        {
            [Test]
            public void Should_return_false()
            {
                IoC.Get<AddAccount>().ChangesTheApplicationSettings().ShouldBeFalse();
            }
        }

        [TestFixture]
        public class When_asked_if_it_changes_the_statement
        {
            [Test]
            public void Should_return_true()
            {
                IoC.Get<AddAccount>().ChangesTheStatement().ShouldBeTrue();
            }
        }

        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_an_account_with_the_provided_name_already_exists : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    var accountType = Init.GetDefaultAccountTypes().First();
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", accountType.Name)
                        .AddAccount("Alpha", accountType.Name)
                        .VerifyStandardErrorMatches(MatchesNoneOf.ErrorMessageText)
                        .VerifyStandardErrorMatches(typeof(IsTheNameOfAnExistingAccount).GetSingularUIDescription());
                }
            }

            [TestFixture]
            public class Given_an_invalid_account_type : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", "xxx")
                        .VerifyStandardErrorMatches(IsTheNameOfAnExistingAccount.NameDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_the_statement_path_has_not_been_initialized : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .AddAccount("Alpha", "Bob")
                        .VerifyStandardErrorMatches(RequireStatement.StatementFilePathNeedsToBeInitializedMessageText);
                }
            }

            [TestFixture]
            public class Given_too_few_arguments : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount()
                        .VerifyStandardErrorMatches(AddAccount.IncorrectParametersMessageText);
                }
            }

            [TestFixture]
            public class Given_too_many_arguments : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", "Bob", "a")
                        .VerifyStandardErrorMatches(AddAccount.IncorrectParametersMessageText);
                }
            }

            [TestFixture]
            public class Given_valid_Execution_Arguments : IntegrationTestBase
            {
                private const string ExpectedAccountName = "Beta";
                private ExecutionArguments _executionArguments;
                private AccountType _expectedAccountType;
                private Notification _result;

                [Test]
                public void Should_add_the_new_account()
                {
                    var statementResult = _executionArguments.Statement;
                    statementResult.HasErrors.ShouldBeFalse();
                    statementResult.Item.Accounts.Count.ShouldBeEqualTo(1);
                    var account = statementResult.Item.Accounts.First();
                    account.Name.ShouldBeEqualTo(ExpectedAccountName);
                    account.AccountType.ShouldBeEqualTo(_expectedAccountType);
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
                    Regex.IsMatch(_result.Infos, AddAccount.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    _executionArguments = Subcutaneous.FromCommandline()
                        .Init(@"x:\previous.statement")
                        .ClearOutput()
                        .CreateExecutionArguments(ExpectedAccountName, Init.GetDefaultAccountTypes().First().Name);

                    _expectedAccountType = _executionArguments.Statement.Item.AccountTypes.First();

                    var command = IoC.Get<AddAccount>();
                    _result = command.Execute(_executionArguments);
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
                    var command = IoC.Get<AddAccount>();
                    command.WriteUsage(writer);
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, AddAccount.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}