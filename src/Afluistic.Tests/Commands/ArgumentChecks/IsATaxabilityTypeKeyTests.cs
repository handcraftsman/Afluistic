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
using Afluistic.Domain.NamedConstants;
using Afluistic.MvbaCore;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.ArgumentChecks
{
    public class IsATaxabilityTypeKeyTests
    {
        public class When_asked_to_check_a_specific_argument
        {
            [TestFixture]
            public class Given_and_invalid_taxability_type_key : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, IsATaxabilityTypeKey.InvalidTaxabilityType.MessageTextToRegex()).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var validator = IoC.Get<IsATaxabilityTypeKey>();
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { "Bob", "uncle" },
                            Statement = new Statement()
                        };
                    _result = validator.Check(executionArguments, 1);
                }
            }

            [TestFixture]
            public class Given_and_valid_taxability_type_key : IntegrationTestBase
            {
                private Notification _result;

                [Test]
                public void Should_return_a_success_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                protected override void Before_first_test()
                {
                    var validator = IoC.Get<IsATaxabilityTypeKey>();
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { "Bob", TaxabilityType.Taxfree.Key },
                            Statement = new Statement()
                        };
                    _result = validator.Check(executionArguments, 1);
                }
            }
        }
    }
}