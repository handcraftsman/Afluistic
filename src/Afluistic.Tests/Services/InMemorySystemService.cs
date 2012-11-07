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

using Afluistic.Services;

namespace Afluistic.Tests.Services
{
    public class InMemorySystemService : ISystemService
    {
        private static StringWriter _standardError = new StringWriter();
        private static StringWriter _standardOut = new StringWriter();
        private DateTime? _currentDateTime;
        public string StandardErrorText
        {
            get { return _standardError.ToString(); }
        }
        public string StandardOutText
        {
            get { return _standardOut.ToString(); }
        }
        public string AppDataDirectory
        {
            get { return @"k:\temp"; }
        }
        public DateTime CurrentDateTime
        {
            get { return _currentDateTime == null ? DateTime.Now : _currentDateTime.Value; }
            set { _currentDateTime = value; }
        }
        public TextWriter StandardError
        {
            get { return _standardError; }
        }
        public TextWriter StandardOut
        {
            get { return _standardOut; }
        }

        public void Reset()
        {
            _standardError.Close();
            _standardError = new StringWriter();
            _standardOut.Close();
            _standardOut = new StringWriter();
        }
    }
}