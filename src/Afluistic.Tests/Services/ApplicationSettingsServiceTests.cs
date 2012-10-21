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

using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Services;

using FluentAssert;

using NUnit.Framework;

using Rhino.Mocks;

using StructureMap.AutoMocking;

namespace Afluistic.Tests.Services
{
    public class ApplicationSettingsServiceTests
    {
        public class When_asked_to_load_the_settings
        {
            [TestFixture]
            public class Given_no_errors_occur
            {
                private ApplicationSettings _applicationSettings;
                private Notification<ApplicationSettings> _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<ApplicationSettingsService>();
                    mocker.Get<ISystemService>()
                        .Expect(x => x.AppDataDirectory)
                        .Return(@"x:\temp");
                    _applicationSettings = new ApplicationSettings();
                    mocker.Get<ISerializationService>()
                        .Expect(x => x.DeserializeFromFile<ApplicationSettings>(Arg<string>.Is.NotNull))
                        .Return(new Notification<ApplicationSettings>
                            {
                                Item = _applicationSettings
                            })
                        .Repeat.Any();
                    _result = mocker.ClassUnderTest.Load();
                }

                [Test]
                public void Should_return_a_success_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                [Test]
                public void Should_return_the_stored_settings_object()
                {
                    _result.Item.ShouldBeSameInstanceAs(_applicationSettings);
                }
            }

            [TestFixture]
            public class Given_the_settings_file_does_not_exist
            {
                private Notification<ApplicationSettings> _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<ApplicationSettingsService>();
                    mocker.Get<ISystemService>()
                        .Expect(x => x.AppDataDirectory)
                        .Return(@"x:\temp");
                    mocker.Get<ISerializationService>()
                        .Expect(x => x.DeserializeFromFile<ApplicationSettings>(Arg<string>.Is.NotNull))
                        .Return(Notification.ErrorFor("pretend").ToNotification<ApplicationSettings>())
                        .Repeat.Any();
                    mocker.Get<IFileSystemService>()
                        .Expect(x => x.FileExists(Arg<string>.Is.NotNull))
                        .Return(false);
                    _result = mocker.ClassUnderTest.Load();
                }

                [Test]
                public void Should_not_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeFalse();
                }

                [Test]
                public void Should_return_a_minimal_settings_object()
                {
                    _result.Item.ShouldNotBeNull();
                }

                [Test]
                public void Should_return_a_warning_notification()
                {
                    _result.HasWarnings.ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_the_settings_file_exists_but_an_error_occurs_during_deserialization
            {
                private Notification<ApplicationSettings> _result;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<ApplicationSettingsService>();
                    mocker.Get<ISystemService>()
                        .Expect(x => x.AppDataDirectory)
                        .Return(@"x:\temp");
                    mocker.Get<ISerializationService>()
                        .Expect(x => x.DeserializeFromFile<ApplicationSettings>(Arg<string>.Is.NotNull))
                        .Return(Notification.ErrorFor("pretend").ToNotification<ApplicationSettings>())
                        .Repeat.Any();
                    mocker.Get<IFileSystemService>()
                        .Expect(x => x.FileExists(Arg<string>.Is.NotNull))
                        .Return(true);
                    _result = mocker.ClassUnderTest.Load();
                }

                [Test]
                public void Should_not_return_a_minimal_settings_object()
                {
                    _result.Item.ShouldBeNull();
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    _result.HasErrors.ShouldBeTrue();
                    _result.Errors.ShouldContain("pretend");
                }
            }
        }

        public class When_asked_to_save_the_settings
        {
            [TestFixture]
            public class Given_an_object_to_save : IntegrationTestBase
            {
                private readonly ApplicationSettings _applicationSettings = new ApplicationSettings
                    {
                        StatementPath = "@k:\test.statement"
                    };

                [Test]
                public void Should_save_the_contents()
                {
                    var settings = base.Settings;
                    settings.HasErrors.ShouldBeFalse();
                    settings.HasWarnings.ShouldBeFalse();
                    settings.Item.StatementPath.ShouldBeEqualTo(_applicationSettings.StatementPath);
                }

                protected override void Before_first_test()
                {
                    IoC.Get<ApplicationSettingsService>().Save(_applicationSettings);
                }
            }
        }
    }
}