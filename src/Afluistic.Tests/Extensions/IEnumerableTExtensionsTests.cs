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

using Afluistic.Extensions;

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Extensions
{
    public class IEnumerableTExtensionsTests
    {
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