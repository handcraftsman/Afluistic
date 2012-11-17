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

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.Prerequisites
{
    public class RequireTaxReportingCategoriesExistTests
    {
        public class When_asked_to_Check
        {
            [TestFixture]
            public class Given_Execution_Arguments_with_a_Statement_result
            {
                [Test]
                public void Should_return_a_success_notification_if_the_Statement_has_at_least_one_tax_reporting_category()
                {
                    var taxReportingCategories = new List<TaxReportingCategory>
                        {
                            new TaxReportingCategory()
                        };
                    var statement = new Statement
                        {
                            TaxReportingCategories = taxReportingCategories
                        };
                    var executionArguments = new ExecutionArguments
                        {
                            Statement = Notification.Empty.ToNotification(statement)
                        };
                    var result = new RequireTaxReportingCategoriesExist().Check(executionArguments);
                    result.IsValid.ShouldBeTrue();
                }

                [Test]
                public void Should_return_an_error_notification_if_the_Statement_has_no_tax_reporting_categories()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Statement = new Notification<Statement>
                                {
                                    Item = new Statement()
                                }
                        };
                    var result = new RequireTaxReportingCategoriesExist().Check(executionArguments);
                    result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(result.Errors, RequireTaxReportingCategoriesExist.NoTaxReportingCategoriesMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}