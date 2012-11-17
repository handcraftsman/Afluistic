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
using Afluistic.Commands.ArgumentChecks;
using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.ArgumentChecks
{
    public class IsTheNameOfAnExistingTaxReportingCategoryTests
    {
        public class When_asked_to_check_a_specific_argument
        {
            [TestFixture]
            public class Given_a_tax_reporting_category_with_the_provided_name_does_not_exist : IntegrationTestBase
            {
                private const string TaxReportingCategoryName = @"Bob";
                private Notification _result;

                [Test]
                public void Should_return_a_failure_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, IsTheNameOfAnExistingTaxReportingCategory.NameDoesNotExistMessageText.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var command = IoC.Get<IsTheNameOfAnExistingTaxReportingCategory>();
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { TaxReportingCategoryName },
                            Statement = new Statement()
                        };
                    _result = command.Check(executionArguments, 0);
                }
            }

            [TestFixture]
            public class Given_a_tax_reporting_category_with_the_provided_name_exists : IntegrationTestBase
            {
                private const string TaxReportingCategoryName = @"Bob";
                private Notification _result;

                [Test]
                public void Should_return_a_success_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                protected override void Before_first_test()
                {
                    var validator = IoC.Get<IsTheNameOfAnExistingTaxReportingCategory>();
                    var statement = new Statement();
                    statement.TaxReportingCategories.Add(new TaxReportingCategory
                        {
                            Name = TaxReportingCategoryName
                        });
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { TaxReportingCategoryName },
                            Statement = statement
                        };
                    _result = validator.Check(executionArguments, 0);
                }
            }
        }
    }
}