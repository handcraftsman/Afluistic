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

using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.ArgumentChecks
{
    public class IsTheNameOfAnExistingAccount : IArgumentValidator
    {
        public const string NameDoesNotExistMessageText = "An {0} with that name does not exist.";

        public Notification Check(ExecutionArguments executionArguments, int argumentIndex)
        {
            var argument = executionArguments.Args[argumentIndex];

            Statement statement = executionArguments.Statement;
            if (!statement.Accounts.Any(x => x.Name == argument))
            {
                return Notification.ErrorFor(NameDoesNotExistMessageText, typeof(Account).GetSingularUIDescription());
            }

            return Notification.Empty;
        }
    }
}