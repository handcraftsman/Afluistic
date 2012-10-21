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

using System.Linq;

using Afluistic.Commands;
using Afluistic.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests
{
    public class ProgramTests
    {
        [TestFixture]
        public class Given_command_line_values_that_do_not_match_a_command : IntegrationTestBase
        {
            [Test]
            public void Should_not_write_to_standard_out()
            {
                StandardOutText.Length.ShouldBeEqualTo(0);
            }

            [Test]
            public void Should_write_an_error_message_to_standard_error()
            {
                StandardErrorText.ShouldContain("namrepus");
            }

            protected override void Before_first_test()
            {
                var program = IoC.Get<Program>();
                program.Handle(new[] { "namrepus" });
            }
        }

        [TestFixture]
        public class Given_command_line_values_that_match_a_command_but_not_its_parameters : IntegrationTestBase
        {
            [Test]
            public void Should_not_write_to_standard_out()
            {
                StandardOutText.Length.ShouldBeEqualTo(0);
            }

            [Test]
            public void Should_write_an_error_message_to_standard_error()
            {
                StandardErrorText.Length.ShouldBeGreaterThan(0);
            }

            protected override void Before_first_test()
            {
                var program = IoC.Get<Program>();
                program.Handle(IoC.Get<Init>().GetCommandWords());
            }
        }

        [TestFixture]
        public class Given_command_line_values_that_match_a_command_with_valid_parameters : IntegrationTestBase
        {
            private const string FilePath = @"x:\data";

            [Test]
            public void Should_not_write_to_standard_error()
            {
                StandardErrorText.Length.ShouldBeEqualTo(0);
            }

            [Test]
            public void Should_write_a_success_message_to_standard_out()
            {
                StandardOutText.Length.ShouldNotBeEqualTo(0);
            }

            protected override void Before_first_test()
            {
                var program = IoC.Get<Program>();

                program.Handle(IoC.Get<Init>().GetCommandWords().Concat(new[] { FilePath }).ToArray());
            }
        }

        [TestFixture]
        public class Given_no_command_line_values : IntegrationTestBase
        {
            [Test]
            public void Should_not_write_to_standard_error()
            {
                StandardErrorText.Length.ShouldBeEqualTo(0);
            }

            [Test]
            public void Should_write_usage_information_to_standard_out()
            {
                StandardOutText.ShouldContain("Usage:");
            }

            protected override void Before_first_test()
            {
                var program = IoC.Get<Program>();
                program.Handle(new string[] { });
            }
        }
    }
}