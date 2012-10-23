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

using Afluistic.MvbaCore;

namespace Afluistic.Extensions
{
    public static class StringExtensions
    {
        public const string ErrorConvertingToAbsolutePathMesssageText = "Don't know how to convert '{0}' to an absolute path. Try using an absolute path.";

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