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
using System.Collections.Generic;
using System.Linq;

namespace Afluistic.Extensions
{
    public static class IEnumerableTExtensions
    {
        public static IEnumerable<IndexedItem<T>> GetIndexedValues<T>(this IEnumerable<T> items)
        {
            var index = 1;
            return items.Select(item => new IndexedItem<T>(index++, item));
        }
    }
}