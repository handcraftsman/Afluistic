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

using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Services;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

using Rhino.Mocks;

using StructureMap.AutoMocking;

namespace Afluistic.Tests.Services
{
    public class StorageServiceTests
    {
        public class When_asked_to_save
        {
            [TestFixture]
            public class Given_an_error_occurs_during_serialization
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<StorageService>();
                    mocker.Get<IApplicationSettingsService>()
                        .Expect(x => x.Load())
                        .Return(new Notification<ApplicationSettings>
                            {
                                Item = new ApplicationSettings
                                    {
                                        StatementPath = @"x:\test.statement"
                                    }
                            })
                        .Repeat.Any();
                    mocker.Get<ISerializationService>()
                        .Expect(x => x.SerializeToFile(Arg<Statement>.Is.NotNull, Arg<string>.Is.NotNull))
                        .Return(Notification.ErrorFor("pretend"));
                    _result = mocker.ClassUnderTest.Save(new Statement());
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    _result.Errors.ShouldContain("pretend");
                }
            }

            [TestFixture]
            public class Given_no_errors_occur : IntegrationTestBase
            {
                private const string StatementPath = @"x:\test.statement";
                private Notification _result;

                [Test]
                public void Should_return_a_success_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                [Test]
                public void Should_store_the_Statement_in_the_configured_location()
                {
                    FileExists(StatementPath).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var applicationSettingsService = IoC.Get<IApplicationSettingsService>();
                    ApplicationSettings applicationSettings = applicationSettingsService.Load();
                    applicationSettings.StatementPath = StatementPath;
                    applicationSettingsService.Save(applicationSettings);

                    var storageService = IoC.Get<IStorageService>();
                    var statement = new Statement();
                    _result = storageService.Save(statement);
                }
            }

            [TestFixture]
            public class Given_the_Statement_filepath_has_not_been_initialized
            {
                private Notification _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<StorageService>();
                    mocker.Get<IApplicationSettingsService>()
                        .Expect(x => x.Load())
                        .Return(new Notification<ApplicationSettings>
                            {
                                Item = new ApplicationSettings
                                    {
                                        StatementPath = null
                                    }
                            })
                        .Repeat.Any();
                    _result = mocker.ClassUnderTest.Save(new Statement());
                }

                [Test]
                public void Should_return_an_error_notification_suggesting_initialization()
                {
                    _result.HasErrors.ShouldBeTrue();
                    Regex.IsMatch(_result.Errors, StorageService.InitializationErrorMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }
        }
    }
}