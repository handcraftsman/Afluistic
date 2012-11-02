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
using Afluistic.Commands.ArgumentChecks;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Commands.ArgumentChecks
{
    public class IsAFilePathTests
    {
        public class When_asked_to_check_a_specific_argument
        {
            [TestFixture]
            public class Given_an_argument_containing_a_valid_filepath : IntegrationTestBase
            {
                private readonly string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                private Notification _result;

                [Test]
                public void Should_return_a_success_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                protected override void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { _filePath },
                        };
                    var validator = IoC.Get<IsAFilePath>();
                    _result = validator.Check(executionArguments, 0);
                }
            }

            [TestFixture]
            public class Given_an_argument_containing_an_invalid_filepath : IntegrationTestBase
            {
                private const string FilePath = @"?";
                private Notification _result;

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, Extensions.StringExtensions.MessageTextToRegex(StringExtensions.ErrorConvertingToAbsolutePathMesssageText)).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var executionArguments = new ExecutionArguments
                        {
                            Args = new[] { FilePath },
                        };
                    var validator = IoC.Get<IsAFilePath>();
                    _result = validator.Check(executionArguments, 0);
                }
            }
        }
    }
}