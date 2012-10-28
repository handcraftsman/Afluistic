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

using Afluistic.Services;
using Afluistic.Tests.TestObjects;

using FluentAssert;

using Newtonsoft.Json;

using NUnit.Framework;

using JsonSerializer = Afluistic.Services.JsonSerializer;

namespace Afluistic.Tests.Services
{
    public class JsonSerializerTests
    {
        public class When_asked_to_deserialize_and_object_from_a_file
        {
            [TestFixture]
            public class Given_a_valid_path : IntegrationTestBase
            {
                private const string Path = @"a:\storage.settings";
                private readonly TestObject _input = new TestObject
                    {
                        Value = @"x:\test"
                    };

                [Test]
                public void Should_deserialize_the_object()
                {
                    var serializer = IoC.Get<JsonSerializer>();
                    var result = serializer.DeserializeFromFile<TestObject>(Path);
                    result.Value.ShouldBeEqualTo(_input.Value);
                }

                protected override void Before_first_test()
                {
                    var serialized = JsonConvert.SerializeObject(_input);
                    using (var writer = IoC.Get<IFileSystemService>().GetStreamWriter(Path))
                    {
                        writer.Write(serialized);
                    }
                }
            }

            [TestFixture]
            public class Given_an_invalid_path : IntegrationTestBase
            {
                private const string Path = @"a:\storage.settings";

                [Test]
                [ExpectedException]
                public void Should_throw_an_exception()
                {
                    var serializer = IoC.Get<JsonSerializer>();
                    serializer.DeserializeFromFile<TestObject>(Path);
                }
            }
        }

        public class When_asked_to_serialize_an_object_to_a_file
        {
            [TestFixture]
            public class Given_a_path_and_an_object_to_serialize : IntegrationTestBase
            {
                private const string Path = @"a:\storage.settings";
                private readonly TestObject _input = new TestObject
                    {
                        Value = @"x:\test"
                    };

                [Test]
                public void Should_serialize_the_object_contents()
                {
                    var contents = IoC.Get<IFileSystemService>().GetStreamReader(Path).ReadToEnd();
                    var result = JsonConvert.DeserializeObject<TestObject>(contents);
                    result.Value.ShouldBeEqualTo(_input.Value);
                }

                [Test]
                public void Should_serialize_to_the_given_location()
                {
                    FileExists(Path).ShouldBeTrue();
                }

                protected override void Before_first_test()
                {
                    var serializer = IoC.Get<JsonSerializer>();
                    serializer.SerializeToFile(Path, _input);
                }
            }
        }
    }
}