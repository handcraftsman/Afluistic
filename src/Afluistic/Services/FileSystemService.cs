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
using System.IO;

namespace Afluistic.Services
{
    public interface IFileSystemService
    {
        bool FileExists(string path);
        TextReader GetStreamReader(string path);
        TextWriter GetStreamWriter(string path);
    }

    public class FileSystemService : IFileSystemService
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public TextReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }

        public TextWriter GetStreamWriter(string path)
        {
            return new StreamWriter(path);
        }
    }
}