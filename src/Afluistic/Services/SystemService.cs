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
using System.IO;

namespace Afluistic.Services
{
    public interface ISystemService
    {
        string AppDataDirectory { get; }
        DateTime CurrentDateTime { get; }
        TextWriter StandardError { get; }
        TextWriter StandardOut { get; }
    }

    public class SystemService : ISystemService
    {
        public TextWriter StandardOut
        {
            get { return Console.Out; }
        }
        public DateTime CurrentDateTime
        {
            get { return DateTime.Now; }
        }
        public TextWriter StandardError
        {
            get { return Console.Error; }
        }
        public string AppDataDirectory
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); }
        }
    }
}