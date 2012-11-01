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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Afluistic.MvbaCore;

namespace Afluistic.Extensions
{
    public static class StringExtensions
    {
        public const string ErrorConvertingToAbsolutePathMesssageText = "Don't know how to convert '{0}' to an absolute path. Try using an absolute path.";

        public static string Pluralize(this string input)
        {
            // other variations are YAGNI at this time
            return input.EndsWith("s") ? input + "es" : input + "s";
        }

        public static string ReplaceTypeReferencesWithUIDescriptions(this string uiDescription, bool plural)
        {
            var parts = uiDescription.Split('$');
            if (parts.Length == 1)
            {
                return uiDescription;
            }
            var result = new StringBuilder();
            result.Append(parts[0]);
            foreach (var part in parts.Skip(1))
            {
                var end = part.IndexOf(' ');
                if (end == -1)
                {
                    end = part.Length;
                }
                var typeName = part.Substring(0, end);
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.Name == typeName);
                if (type != null)
                {
                    var typeDescription = plural ? type.GetPluralUIDescription() : type.GetSingularUIDescription();
                    result.Append(typeDescription);
                    result.Append(part.Substring(typeName.Length));
                }
                else
                {
                    result.Append("$" + part);
                }
            }
            return result.ToString();
        }

        public static string[] SplitOnTransitionToCapitalLetter(this string input)
        {
            var result = input.Group((curr, prev) => Char.IsUpper(prev) || Char.IsLower(curr))
                .Select(x => new String(x.ToArray()))
                .ToArray();
            return result;
        }

        public static Notification<string> ToAbsolutePath(this string path)
        {
            if (path.Contains(":"))
            {
                return path;
            }
            if (path.StartsWith("..") || path.StartsWith(@".\"))
            {
                return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
            }
            if (path == ".")
            {
                return Environment.CurrentDirectory;
            }
            return Notification.ErrorFor(ErrorConvertingToAbsolutePathMesssageText, path).ToNotification<string>();
        }

        public static Notification<string> ToStatementPath(this string path)
        {
            var absolutePath = path.ToAbsolutePath();
            if (absolutePath.HasErrors)
            {
                return absolutePath;
            }
            if (Directory.Exists(absolutePath))
            {
                return Path.Combine(absolutePath, Constants.DefaultStatementFileName);
            }
            return absolutePath;
        }
    }
}