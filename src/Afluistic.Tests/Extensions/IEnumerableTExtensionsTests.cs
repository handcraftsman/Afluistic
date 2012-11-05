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
using System.Linq;

using Afluistic.Extensions;
using Afluistic.Tests.TestObjects;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class IEnumerableTExtensionsTests
    {
        public class When_asked_to_get_by_property_value_or_index
        {
            [TestFixture]
            public class Given_a_non_integer_value_that_matches_a_property_value
            {
                [Test]
                public void Should_throw_an_Exception()
                {
                    var input = new[]
                        {
                            new TestObject
                                {
                                    Value = "A"
                                },
                            new TestObject
                                {
                                    Value = "B"
                                },
                        };
                    var result = input.GetByPropertyValueOrIndex(x => x.Value, "B");
                    result.ShouldBeSameInstanceAs(input[1]);
                }
            }

            [TestFixture]
            public class Given_an_integer_value_that_does_not_match_a_property_value_or_index
            {
                [Test]
                [ExpectedException(typeof(InvalidOperationException))]
                public void Should_throw_an_Exception()
                {
                    var input = new TestObject[] { };
                    input.GetByPropertyValueOrIndex(x => x.Value, "6");
                }
            }

            [TestFixture]
            public class Given_an_integer_value_that_matches_a_property_value
            {
                [Test]
                public void Should_throw_an_Exception()
                {
                    var input = new[]
                        {
                            new TestObject
                                {
                                    Value = "3"
                                },
                            new TestObject
                                {
                                    Value = "6"
                                },
                        };
                    var result = input.GetByPropertyValueOrIndex(x => x.Value, "6");
                    result.ShouldBeSameInstanceAs(input[1]);
                }
            }

            [TestFixture]
            public class Given_an_integer_value_that_matches_an_index
            {
                [Test]
                public void Should_throw_an_Exception()
                {
                    var input = new[]
                        {
                            new TestObject
                                {
                                    Value = "A"
                                },
                            new TestObject
                                {
                                    Value = "B"
                                },
                        };
                    var result = input.GetByPropertyValueOrIndex(x => x.Value, "2");
                    result.Value.ShouldBeEqualTo("B");
                }
            }
        }

        public class When_asked_to_get_indexed_values
        {
            [TestFixture]
            public class Given_a_single_item_in_the_input
            {
                [Test]
                public void Should_return_the_item_paired_with_an_index_of_1()
                {
                    var input = new[] { 99 };
                    var result = input.GetIndexedValues().ToList();
                    result.Count.ShouldBeEqualTo(input.Length);
                    result.First().Index.ShouldBeEqualTo(1);
                }
            }

            [TestFixture]
            public class Given_an_empty_input
            {
                [Test]
                public void Should_return_no_results()
                {
                    var input = new int[] { };
                    var result = input.GetIndexedValues().ToList();
                    result.Count.ShouldBeEqualTo(0);
                }
            }

            [TestFixture]
            public class Given_multiple_items_in_the_input
            {
                [Test]
                public void Should_return_the_items_paired_with_a_1_up_index()
                {
                    var input = new[] { 99, 101, 103 };
                    var result = input.GetIndexedValues().ToList();
                    result.Count.ShouldBeEqualTo(input.Length);
                    result.Select(x => x.Index).ShouldBeEqualTo(Enumerable.Range(1, input.Length));
                }
            }
        }
    }
}