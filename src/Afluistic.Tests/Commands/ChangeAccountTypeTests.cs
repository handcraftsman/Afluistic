﻿// * **************************************************************************
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
using Afluistic.Domain;
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class ChangeAccountTypeTests
    {
        [TestFixture]
        public class When_asked_if_it_changes_the_application_settings
        {
            [Test]
            public void Should_return_false()
            {
                IoC.Get<ChangeAccountType>().ChangesTheApplicationSettings().ShouldBeFalse();
            }
        }

        [TestFixture]
        public class When_asked_if_it_changes_the_statement
        {
            [Test]
            public void Should_return_true()
            {
                IoC.Get<ChangeAccountType>().ChangesTheStatement().ShouldBeTrue();
            }
        }

        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_the_first_argument_is_not_an_existing_account_index : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", Init.GetDefaultAccountTypes().First().Name)
                        .ChangeAccountType("0", TaxabilityType.Taxable.Key)
                        .VerifyStandardErrorMatches(IsTheIndexOfAnExistingAccount.IndexDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_the_first_argument_is_not_an_existing_account_name : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", Init.GetDefaultAccountTypes().First().Name)
                        .ChangeAccountType("Bob", TaxabilityType.Taxable.Key)
                        .VerifyStandardErrorMatches(IsTheNameOfAnExistingAccount.NameDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_the_second_argument_is_not_an_existing_account_type_index : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", Init.GetDefaultAccountTypes().First().Name)
                        .ChangeAccountType("Alpha", "0")
                        .VerifyStandardErrorMatches(IsTheIndexOfAnExistingAccountType.IndexDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_the_second_argument_is_not_an_existing_account_type_name : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", Init.GetDefaultAccountTypes().First().Name)
                        .ChangeAccountType("Alpha", "Bob")
                        .VerifyStandardErrorMatches(IsTheNameOfAnExistingAccountType.NameDoesNotExistMessageText);
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
                        .ChangeAccountType("Alpha")
                        .VerifyStandardErrorMatches(ChangeAccountType.IncorrectParametersMessageText);
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
                        .ChangeAccountType("Alpha", Init.GetDefaultAccountTypes().First().Name, "a")
                        .VerifyStandardErrorMatches(ChangeAccountType.IncorrectParametersMessageText);
                }
            }

            [TestFixture]
            public class Given_valid_Execution_Arguments : IntegrationTestBase
            {
                private ExecutionArguments _executionArguments;
                private AccountType _expectedAccountType;
                private Notification _result;

                [Test]
                public void Should_change_the_account_type()
                {
                    var statementResult = _executionArguments.Statement;
                    statementResult.HasErrors.ShouldBeFalse();
                    statementResult.Item.Accounts.Count.ShouldBeEqualTo(1);
                    var account = statementResult.Item.Accounts.First();
                    account.Name.ShouldBeEqualTo("Alpha");
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
                    Regex.IsMatch(_result.Infos, ChangeAccountType.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    _expectedAccountType = Init.GetDefaultAccountTypes().Last();
                    _executionArguments = Subcutaneous.FromCommandline()
                        .Init(@"x:\previous.statement")
                        .AddAccount("Alpha", Init.GetDefaultAccountTypes().First().Name)
                        .ClearOutput()
                        .CreateExecutionArguments("Alpha", _expectedAccountType.Name);

                    var command = IoC.Get<ChangeAccountType>();
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
                    var command = IoC.Get<ChangeAccountType>();
                    command.WriteUsage(writer);
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, ChangeAccountType.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}