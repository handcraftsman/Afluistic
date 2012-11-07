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

using Afluistic.Commands;
using Afluistic.Commands.PostConditions;
using Afluistic.MvbaCore;

namespace Afluistic.Tests.TestObjects.Commands
{
    public class CommandThatChangesTheStatement : ICommand, IChangeStatement
    {
        public Notification Execute(ExecutionArguments executionArguments)
        {
            throw new NotImplementedException();
        }

        public void WriteUsage(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}