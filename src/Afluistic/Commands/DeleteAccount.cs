﻿// * **************************************************************************
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
    public class DeleteAccount : ICommand, IChangeStatement
    {
        public const string IncorrectParametersMessageText = "$AccountType or index must be specified.";
        public const string SuccessMessageText = "The {0} was deleted";
        public const string UsageMessageText = "\tPermanently deletes an {0}.";

        [RequireStatement]
        [RequireExactlyNArgs(1, IncorrectParametersMessageText)]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccount), typeof(IsTheIndexOfAnExistingAccount))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var account = statement.Accounts.GetByPropertyValueOrIndex(x => x.Name, executionArguments.Args[0]);
            statement.Accounts.Remove(account);

            return Notification.InfoFor(SuccessMessageText, typeof(Account).GetSingularUIDescription());
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [account name|index#]");
            textWriter.WriteLine(UsageMessageText, typeof(Account).GetSingularUIDescription());
        }
    }
}