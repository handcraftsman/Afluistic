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

using Afluistic.Services;
using Afluistic.Tests.Extensions;

using FluentAssert;

using NUnit.Framework;

using Rhino.Mocks;

using StructureMap.AutoMocking;

namespace Afluistic.Tests.Services
{
    public class SerializationServiceTests
    {
        public class When_asked_to_deserialize_an_object_from_a_file
        {
            [TestFixture]
            public class Given_an_exception_is_thrown
            {
                private ISerializationService _service;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<SerializationService>();
                    mocker.Get<IJsonSerializer>()
                        .Expect(x => x.DeserializeFromFile<TestObject>(Arg<string>.Is.NotNull))
                        .Throw(new Exception("pretend"));
                    _service = mocker.ClassUnderTest;
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    var result = _service.DeserializeFromFile<TestObject>(@"x:\test.settings");
                    result.HasErrors.ShouldBeTrue();
                    result.Errors.ShouldContain("pretend");
                    Regex.IsMatch(result.Errors, SerializationService.DeserializationErrorMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_the_deserialization_succeeds
            {
                private readonly TestObject _objToReturn = new TestObject
                    {
                        Value = "test"
                    };
                private ISerializationService _service;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<SerializationService>();
                    mocker.Get<IJsonSerializer>()
                        .Expect(x => x.DeserializeFromFile<TestObject>(Arg<string>.Is.NotNull))
                        .Return(_objToReturn)
                        .Repeat.Any();
                    _service = mocker.ClassUnderTest;
                }

                [Test]
                public void Should_return_a_success_notification()
                {
                    var result = _service.DeserializeFromFile<TestObject>(@"x:\test.settings");
                    result.HasErrors.ShouldBeFalse();
                }

                [Test]
                public void Should_return_the_deserialized_object()
                {
                    var result = _service.DeserializeFromFile<TestObject>(@"x:\test.settings");
                    result.Item.ShouldBeSameInstanceAs(_objToReturn);
                }
            }
        }

        public class When_asked_to_serialize_an_object_to_a_file
        {
            [TestFixture]
            public class Given_an_exception_is_thrown
            {
                private ISerializationService _service;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    var mocker = new RhinoAutoMocker<SerializationService>();
                    mocker.Get<IJsonSerializer>()
                        .Expect(x => x.SerializeToFile(Arg<string>.Is.NotNull, Arg<TestObject>.Is.NotNull))
                        .Throw(new Exception("pretend"));
                    _service = mocker.ClassUnderTest;
                }

                [Test]
                public void Should_return_an_error_notification()
                {
                    var result = _service.SerializeToFile(new TestObject(), @"x:\test.settings");
                    result.HasErrors.ShouldBeTrue();
                    result.Errors.ShouldContain("pretend");
                    Regex.IsMatch(result.Errors, SerializationService.SerializationErrorMessageText.MessageTextToRegex()).ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_the_serialization_succeeds
            {
                private ISerializationService _service;

                [TestFixtureSetUp]
                public void Before_first_test()
                {
                    _service = new RhinoAutoMocker<SerializationService>().ClassUnderTest;
                }

                [Test]
                public void Should_return_a_success_notification()
                {
                    var result = _service.SerializeToFile(new TestObject(), @"x:\test.settings");
                    result.HasErrors.ShouldBeFalse();
                }
            }
        }
    }
}