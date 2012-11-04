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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Commands.ArgumentChecks;
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
    public class ChangeAccountTypeTaxabilityTypeTests
    {
        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_the_first_argument_is_not_an_existing_account_type : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .ChangeAccountTypeTaxabilityType("Savings", TaxabilityType.Taxable.Key)
                        .VerifyStandardErrorMatches(IsTheNameOfAnExistingAccountType.NameDoesNotExistMessageText);
                }
            }

            [TestFixture]
            public class Given_the_second_argument_is_not_a_taxability_type : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .AddAccountType("Savings", TaxabilityType.Taxfree.Key)
                        .ChangeAccountTypeTaxabilityType("Savings", "z")
                        .VerifyStandardErrorMatches(IsATaxabilityTypeKey.InvalidTaxabilityType);
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
                        .ChangeAccountTypeTaxabilityType("Savings")
                        .VerifyStandardErrorMatches(ChangeAccountTypeTaxabilityType.IncorrectParametersMessageText);
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
                        .ChangeAccountTypeTaxabilityType("Savings", TaxabilityType.Taxable.Key, "a")
                        .VerifyStandardErrorMatches(ChangeAccountTypeTaxabilityType.IncorrectParametersMessageText);
                }
            }

            [TestFixture]
            public class Given_valid_Execution_Arguments : IntegrationTestBase
            {
                private const string ExpectedAccountName = "Bob";
                private TaxabilityType _expectedTaxabilityType = TaxabilityType.Taxable;
                private Notification _result;

                [Test]
                public void Should_change_the_taxability_type()
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
                    Regex.IsMatch(_result.Infos, ChangeAccountTypeTaxabilityType.SuccessMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var executionArguments = Subcutaneous.FromCommandline()
                        .Init(@"x:\previous.statement")
                        .AddAccountType(ExpectedAccountName, TaxabilityType.GetAll().First(x => x != _expectedTaxabilityType).Key)
                        .ClearOutput()
                        .CreateExecutionArguments(ExpectedAccountName, _expectedTaxabilityType.Key);

                    var command = IoC.Get<ChangeAccountTypeTaxabilityType>();
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
                    var command = IoC.Get<ChangeAccountTypeTaxabilityType>();
                    command.WriteUsage(writer);
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, ChangeAccountTypeTaxabilityType.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}