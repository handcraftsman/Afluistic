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
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands
{
    public class ListTaxReportingCategoriesTests
    {
        [TestFixture]
        public class When_asked_if_it_changes_the_application_settings
        {
            [Test]
            public void Should_return_false()
            {
                IoC.Get<ListTaxReportingCategories>().ChangesTheApplicationSettings().ShouldBeFalse();
            }
        }

        [TestFixture]
        public class When_asked_if_it_changes_the_statement
        {
            [Test]
            public void Should_return_false()
            {
                IoC.Get<ListTaxReportingCategories>().ChangesTheStatement().ShouldBeFalse();
            }
        }

        public class When_asked_to_execute
        {
            [TestFixture]
            public class Given_Execution_Arguments : IntegrationTestBase
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
                    StandardOutText.ShouldContain(Init.GetDefaultTaxReportingCategories().First().Name);
                }

                protected override void Before_first_test()
                {
                    var executionArguments = Subcutaneous.FromCommandline()
                        .Init("x:")
                        .ClearOutput()
                        .CreateExecutionArguments();

                    var command = IoC.Get<ListTaxReportingCategories>();
                    _result = command.Execute(executionArguments);
                }
            }

            [TestFixture]
            public class Given_any_arguments : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .Init("x:")
                        .ListTaxReportingCategories("a")
                        .VerifyStandardErrorMatches(RequireExactlyNArgs.WrongNumberOfArgumentsMessageText);
                }
            }

            [TestFixture]
            public class Given_no_tax_reporting_categories_exist : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    var subcutaneous = Subcutaneous.FromCommandline()
                        .Init("x:");
                    Statement statement = Statement;
                    statement.TaxReportingCategories.Clear();
                    IoC.Get<IStorageService>().Save(statement);
                    subcutaneous.ListTaxReportingCategories()
                        .VerifyStandardErrorMatches(RequireTaxReportingCategoriesExist.NoTaxReportingCategoriesMessageText);
                }
            }

            [TestFixture]
            public class Given_the_statement_path_has_not_been_initialized : IntegrationTestBase
            {
                [Test]
                public void Should_return_the_correct_error_message()
                {
                    Subcutaneous.FromCommandline()
                        .ListTaxReportingCategories()
                        .VerifyStandardErrorMatches(RequireStatement.StatementFilePathNeedsToBeInitializedMessageText);
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
                    var command = IoC.Get<ListTaxReportingCategories>();
                    command.WriteUsage(writer);
                    var output = writer.ToString();
                    output.ShouldContain(String.Join(" ", command.GetCommandWords()));
                    Regex.IsMatch(output, ListTaxReportingCategories.UsageMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}