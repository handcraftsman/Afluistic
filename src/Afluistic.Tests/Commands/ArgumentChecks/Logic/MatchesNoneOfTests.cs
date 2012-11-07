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
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Extensions;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.ArgumentChecks.Logic
{
    public class MatchesNoneOfTests
    {
        public class When_applied_to_specific_argument_validators
        {
            [TestFixture]
            public class Given_all_validators_return_failure_notifications : IntegrationTestBase
            {
                [Test]
                public void Should_return_a_success_notification()
                {
                    var logicModifier = IoC.Get<MatchesNoneOf>();
                    var result = logicModifier.ApplyTo(new ExecutionArguments(), 0, new[] { typeof(AlwaysReturnsFailureValidator) });
                    result.HasErrors.ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_no_validators : IntegrationTestBase
            {
                [Test]
                public void Should_return_a_success_notification()
                {
                    var logicModifier = IoC.Get<MatchesNoneOf>();
                    var result = logicModifier.ApplyTo(new ExecutionArguments(), 0, new Type[] { });
                    result.HasErrors.ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_the_final_validator_returns_a_success_notification : IntegrationTestBase
            {
                [Test]
                public void Should_return_a_failure_notification()
                {
                    var logicModifier = IoC.Get<MatchesNoneOf>();
                    var result = logicModifier.ApplyTo(new ExecutionArguments(), 0, new[] { typeof(AlwaysReturnsFailureValidator), typeof(AlwaysReturnsSuccessValidator) });
                    result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(result.Errors, MatchesNoneOf.ErrorMessageText.MessageTextToRegex()).ShouldBeTrue();
                    result.Errors.ShouldContain(typeof(AlwaysReturnsSuccessValidator).GetTypeNameWordsAsString());
                }
            }
        }
    }
}