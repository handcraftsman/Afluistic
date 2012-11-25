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
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class SelectAccountTests
    {
        [TestFixture]
        public class When_asked_if_it_changes_the_application_settings
        {
            [Test]
            public void Should_return_false()
            {
                IoC.Get<SelectAccount>().ChangesTheApplicationSettings().ShouldBeFalse();
            }
        }

        [TestFixture]
        public class When_asked_if_it_changes_the_statement
        {
            [Test]
            public void Should_return_true()
            {
                IoC.Get<SelectAccount>().ChangesTheStatement().ShouldBeTrue();
            }
        }

        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_a_non_existant_account_name : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .SelectAccount("Alpha")
                        .VerifyStandardErrorMatches(IsTheNameOfAnExistingAccount.NameDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_an_account_with_the_provided_index_exists : IntegrationTestBase
            {
                [Test]
                public void Should_make_that_account_the_selected_account_in_the_statement()
                {
                    Statement statement = Statement;
                    statement.Accounts.Count.ShouldBeEqualTo(3);
                    statement.SelectedAccount.ShouldBeSameInstanceAs(statement.Accounts.First(x => x.Name == "Beta"));
                }

                [Test]
                public void Should_return_a_success_message()
                {
                    StandardErrorText.ShouldBeEqualTo("");
                    Regex.IsMatch(StandardOutText, SelectAccount.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var accountType = Init.GetDefaultAccountTypes().First();
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", accountType.Name)
                        .AddAccount("Beta", accountType.Name)
                        .AddAccount("Gamma", accountType.Name)
                        .SelectAccount("2");
                }
            }

            [TestFixture]
            public class Given_an_account_with_the_provided_name_exists : IntegrationTestBase
            {
                [Test]
                public void Should_make_that_account_the_selected_account_in_the_statement()
                {
                    var accountType = Init.GetDefaultAccountTypes().First();
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccount("Alpha", accountType.Name)
                        .AddAccount("Beta", accountType.Name)
                        .AddAccount("Gamma", accountType.Name)
                        .SelectAccount("Beta");

                    Statement statement = Statement;
                    statement.Accounts.Count.ShouldBeEqualTo(3);
                    statement.SelectedAccount.ShouldBeSameInstanceAs(statement.Accounts.First(x => x.Name == "Beta"));
                }
            }

            [TestFixture]
            public class Given_the_statement_path_has_not_been_initialized : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .SelectAccount("Alpha")
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
                        .SelectAccount()
                        .VerifyStandardErrorMatches(SelectAccount.IncorrectParametersMessageText);
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
                        .SelectAccount("Alpha", "a")
                        .VerifyStandardErrorMatches(SelectAccount.IncorrectParametersMessageText);
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
                    var command = IoC.Get<SelectAccount>();
                    command.WriteUsage(writer);
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, SelectAccount.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}