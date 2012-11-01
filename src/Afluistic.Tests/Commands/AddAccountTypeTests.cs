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
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class AddAccountTypeTests
    {
        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_an_account_type_with_the_provided_name_already_exists : IntegrationTestBase
            {
                private const string AccountTypeName = @"Bob";
                private Notification _result;

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, AddAccountType.NameAlreadyExistsMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<AddAccountType>();
                    var statement = new Statement();
                    statement.AccountTypes.Add(new AccountType
                        {
                            Name = AccountTypeName
                        });
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { AccountTypeName, null },
                            Statement = statement
                        };
                    _result = command.Execute(executionArguments);
                }
            }

            [TestFixture]
            public class Given_and_invalid_taxability_type : IntegrationTestBase
            {
                private const string AccountTypeName = @"Bob";
                private Notification _result;

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, AddAccountType.InvalidTaxabilityType.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<AddAccountType>();
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { AccountTypeName, "uncle" },
                            Statement = new Statement()
                        };
                    _result = command.Execute(executionArguments);
                }
            }

            [TestFixture]
            public class Given_valid_Execution_Arguments : IntegrationTestBase
            {
                private const string ExpectedAccountName = "Bob";
                private TaxabilityType _expectedTaxabilityType;
                private Notification _result;

                [Test]
                public void Should_add_the_new_account_type()
                {
                    var statementResult = Statement;
                    statementResult.HasErrors.ShouldBeFalse();
                    statementResult.Item.AccountTypes.Count.ShouldBeEqualTo(1);
                    var accountType = statementResult.Item.AccountTypes.First();
                    accountType.Name.ShouldBeEqualTo(ExpectedAccountName);
                    accountType.Taxability.ShouldBeEqualTo(_expectedTaxabilityType);
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
                    Regex.IsMatch(_result.Infos, AddAccountType.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    _expectedTaxabilityType = TaxabilityType.Taxable;
                    var executionArguments = new ExecutionArguments
                        {
                            ApplicationSettings = new ApplicationSettings
                                {
                                    StatementPath = @"x:\previous.statement"
                                },
                            Args = new[] { ExpectedAccountName, _expectedTaxabilityType.Key },
                            Statement = new Statement()
                        };
                    var configured = IoC.Get<IApplicationSettingsService>().Save(executionArguments.ApplicationSettings);
                    configured.IsValid.ShouldBeTrue(() => configured.ErrorsAndWarnings);
                    var stored = IoC.Get<IStorageService>().Save(executionArguments.Statement);
                    stored.IsValid.ShouldBeTrue(() => configured.ErrorsAndWarnings);

                    var command = IoC.Get<AddAccountType>();
                    _result = command.Execute(executionArguments);
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