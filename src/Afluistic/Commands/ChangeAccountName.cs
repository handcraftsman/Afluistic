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

using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.PostConditions;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands
{
    public class ChangeAccountName : ICommand, IChangeStatement
    {
        public const string IncorrectParametersMessageText = "Old $Account name or index and a new $Account name must be specified.";
        public const string SuccessMessageText = "The name was changed";
        public const string UsageMessageText = "\tChanges the name of an {0}.";

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccount), typeof(IsTheIndexOfAnExistingAccount))]
        [VerifyThatArgument(2, typeof(MatchesNoneOf), typeof(IsTheNameOfAnExistingAccount))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var account = statement.Accounts.GetByPropertyValueOrIndex(x => x.Name, executionArguments.Args[0]);
            account.Name = executionArguments.Args[1];

            return Notification.InfoFor(SuccessMessageText);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [old name|index#] [new name]");
            textWriter.WriteLine(UsageMessageText, typeof(Account).GetSingularUIDescription());
        }
    }
}