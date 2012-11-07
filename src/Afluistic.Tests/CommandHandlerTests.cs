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
using Afluistic.MvbaCore;
using Afluistic.Tests.TestObjects.Commands;

using FluentAssert;

using NUnit.Framework;

using Rhino.Mocks;

using StructureMap.AutoMocking;

namespace Afluistic.Tests
{
    public class CommandHandlerTests
    {
        public class When_asked_to_handle
        {
            [TestFixture]
            public class Given_all_prerequisites_for_the_command_pass
            {
                private CommandWithNoPrerequisites _command;
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    _command = new CommandWithNoPrerequisites();
                    var mocker = new RhinoAutoMocker<CommandHandler>();
                    mocker.Inject(typeof(ICommand), _command);
                    mocker.Get<IPrerequisiteChecker>()
                        .Expect(x => x.Check(Arg<ICommand>.Is.Same(_command), Arg<ExecutionArguments>.Is.NotNull))
                        .Return(Notification.Empty);
                    _result = mocker.ClassUnderTest.Handle(_command, new ExecutionArguments());
                }

                [Test]
                public void Should_execute_the_command_and_return_its_notification()
                {
                    _result.IsValid.ShouldBeTrue();
                    _result.Infos.ShouldBeEqualTo(CommandWithNoPrerequisites.CommandWasExecutedMessageText);
                }
            }

            [TestFixture]
            public class Given_arguments_that_fail_a_prerequisite_for_the_command
            {
                private CommandWithOnePrerequisite _command;
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    _command = new CommandWithOnePrerequisite();
                    var mocker = new RhinoAutoMocker<CommandHandler>();
                    mocker.Inject(typeof(ICommand), _command);
                    mocker.Get<IPrerequisiteChecker>()
                        .Expect(x => x.Check(Arg<ICommand>.Is.Same(_command), Arg<ExecutionArguments>.Is.NotNull))
                        .Return(Notification.ErrorFor("pretend"));
                    _result = mocker.ClassUnderTest.Handle(_command, new ExecutionArguments());
                }

                [Test]
                public void Should_not_execute_the_command()
                {
                    _result.Infos.ShouldBeEqualTo("");
                }

                [Test]
                public void Should_return_an_error_notification_for_the_failed_prerequisite()
                {
                    _result.Errors.ShouldBeEqualTo("pretend");
                }
            }
        }
    }
}