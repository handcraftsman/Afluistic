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
using System.IO;

using Afluistic.Services;

namespace Afluistic.Tests.Services
{
    public class InMemoryFileSystemService : IFileSystemService
    {
// ReSharper disable InconsistentNaming
        private static readonly Dictionary<string, StringWriter> _fileSystem = new Dictionary<string, StringWriter>();
// ReSharper restore InconsistentNaming

        public bool FileExists(string path)
        {
            return _fileSystem.ContainsKey(path);
        }

        public TextReader GetStreamReader(string path)
        {
            if (!FileExists(path))
            {
                throw new FileNotFoundException("File '" + path + "' does not exist.");
            }
            return new StringReader(_fileSystem[path].ToString());
        }

        public TextWriter GetStreamWriter(string path)
        {
            var stringWriter = new StringWriter();
            if (_fileSystem.ContainsKey(path))
            {
                _fileSystem[path] = stringWriter;
            }
            else
            {
                _fileSystem.Add(path, stringWriter);
            }
            return stringWriter;
        }

        public void Reset()
        {
            foreach (var stream in _fileSystem.Values)
            {
                stream.Close();
            }
            _fileSystem.Clear();
        }
    }
}