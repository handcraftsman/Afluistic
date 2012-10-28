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

using System.Linq;

using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.Prerequisites
{
    public interface IPrerequisiteChecker
    {
        Notification Check(ICommand command, ExecutionArguments executionArguments);
    }

    public class PrerequisiteChecker : IPrerequisiteChecker
    {
        public Notification Check(ICommand command, ExecutionArguments executionArguments)
        {
            var result = command.GetCommandExecutionPrerequisites()
                             .OrderBy(x => x.Order)
                             .Select(x => x.Check(executionArguments))
                             .FirstOrDefault(x => !x.IsValid) ?? Notification.Empty;
            return result;
        }
    }
}