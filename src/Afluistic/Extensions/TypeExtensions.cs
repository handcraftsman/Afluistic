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
using System.Linq.Expressions;

using Afluistic.MvbaCore;

namespace Afluistic.Extensions
{
    public static class TypeExtensions
    {
        public static string[] GetTypeNameWords(this Type type)
        {
            var nameWords = type.Name.SplitOnTransitionToCapitalLetter();
            return nameWords;
        }

        public static string GetUIDescription(this Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(UIDescriptionAttribute), false).FirstOrDefault() as UIDescriptionAttribute;
            if (attribute != null)
            {
                return attribute.UIDescription;
            }
            return type.Name;
        }

        public static string GetUIDescription<T>(Expression<Func<T, object>> propertyOnType)
        {
            var type = typeof(T);
            var expectedPropertyName = propertyOnType.GetFinalPropertyName();
            var property = type.GetProperties().First(x => x.Name == expectedPropertyName);
            var attribute = property.GetCustomAttributes(typeof(UIDescriptionAttribute), false).FirstOrDefault() as UIDescriptionAttribute;
            if (attribute != null)
            {
                return attribute.UIDescription;
            }
            return property.Name;
        }
    }
}