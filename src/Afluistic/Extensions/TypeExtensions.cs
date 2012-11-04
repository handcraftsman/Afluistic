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
        public static string GetPluralUIDescription(this Type type)
        {
            return GetUIDescription(type, true);
        }

        public static string GetPluralUIDescription<T>(Expression<Func<T, object>> propertyOnType)
        {
            return GetUIDescription(propertyOnType, true);
        }

        public static string GetSingularUIDescription(this Type type)
        {
            return GetUIDescription(type, false);
        }

        public static string GetSingularUIDescription<T>(Expression<Func<T, object>> propertyOnType)
        {
            return GetUIDescription(propertyOnType, false);
        }

        public static string[] GetTypeNameWords(this Type type)
        {
            var nameWords = type.Name.SplitOnTransitionToCapitalLetter();
            return nameWords;
        }

        public static string GetTypeNameWordsAsString(this Type type)
        {
            var nameWords = String.Join(" ", type.Name.SplitOnTransitionToCapitalLetter());
            return nameWords;
        }

        private static string GetUIDescription(this Type type, bool plural)
        {
            var attribute = type.GetCustomAttributes(typeof(UIDescriptionAttribute), false).FirstOrDefault() as UIDescriptionAttribute;
            if (attribute != null)
            {
                return plural ? attribute.PluralUIDescription : attribute.UIDescription;
            }
            return plural ? type.GetTypeNameWordsAsString().Pluralize() : type.GetTypeNameWordsAsString();
        }

        private static string GetUIDescription<T>(Expression<Func<T, object>> propertyOnType, bool plural)
        {
            var type = typeof(T);
            var expectedPropertyName = propertyOnType.GetFinalPropertyName();
            var property = type.GetProperties().First(x => x.Name == expectedPropertyName);
            var attribute = property.GetCustomAttributes(typeof(UIDescriptionAttribute), false).FirstOrDefault() as UIDescriptionAttribute;
            if (attribute != null)
            {
                return plural ? attribute.PluralUIDescription : attribute.UIDescription;
            }
            return plural ? property.GetPropertyNameWordsAsString().Pluralize() : property.GetPropertyNameWordsAsString();
        }
    }
}