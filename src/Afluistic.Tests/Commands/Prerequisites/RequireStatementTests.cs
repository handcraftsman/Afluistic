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

using Afluistic.Commands;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.MvbaCore;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.Prerequisites
{
    public class RequireStatementTests
    {
        public class When_asked_to_Check
        {
            [TestFixture]
            public class Given_Execution_Arguments_with_a_Statement_result
            {
                [Test]
                public void Should_return_a_success_notification_if_the_Statement_result_does_not_have_errors()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Statement = Notification.Empty.ToNotification<Statement>()
                        };
                    var result = new RequireStatement().Check(executionArguments);
                    result.IsValid.ShouldBeTrue();
                }

                [Test]
                public void Should_return_an_error_notification_if_the_Statement_result_has_errors()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Statement = Notification.ErrorFor("pretend").ToNotification<Statement>()
                        };
                    var result = new RequireStatement().Check(executionArguments);
                    result.IsValid.ShouldBeFalse();
                }
            }
        }
    }
}