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
    public class ChangeAccountType : ICommand, IChangeStatement
    {
        public const string IncorrectParametersMessageText = "$Account name or index and new $AccountType name or index must be specified.";
        public const string SuccessMessageText = "The {0} was changed";
        public const string UsageMessageText = "\tChanges the {0} of an {1}.";

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccount), typeof(IsTheIndexOfAnExistingAccount))]
        [VerifyThatArgument(2, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccountType), typeof(IsTheIndexOfAnExistingAccountType))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var account = statement.Accounts.GetByPropertyValueOrIndex(x => x.Name, executionArguments.Args[0]);
            var accountType = statement.AccountTypes.GetByPropertyValueOrIndex(x => x.Name, executionArguments.Args[1]);
            account.AccountType = accountType;

            return Notification.InfoFor(SuccessMessageText, typeof(AccountType).GetSingularUIDescription());
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [name|index#] [new (existing) type name|index#]");
            textWriter.WriteLine(UsageMessageText, typeof(AccountType).GetSingularUIDescription(), typeof(Account).GetSingularUIDescription());
        }
    }
}