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
using System.Linq;

using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class ChangeAccountTypeName : ICommand
    {
        public const string IncorrectParametersMessageText = "Old $AccountType name or index and a new $AccountType name must be specified.";
        public const string SuccessMessageText = "The name was changed";
        public const string UsageMessageText = "\tChanges the name of an {0}.";
        private readonly IStorageService _storageService;

        public ChangeAccountTypeName(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccountType), typeof(IsTheIndexOfAnExistingAccountType))]
        [VerifyThatArgument(2, typeof(MatchesNoneOf), typeof(IsTheNameOfAnExistingAccountType))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var accountType = statement.AccountTypes.GetIndexedValues()
                .First(x => x.Item.Name == executionArguments.Args[0] || x.Index.ToString() == executionArguments.Args[0]).Item;
            accountType.Name = executionArguments.Args[1];

            var storageResult = _storageService.Save(statement);
            if (storageResult.HasErrors)
            {
                return storageResult;
            }
            return Notification.InfoFor(SuccessMessageText);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [old type name|index#] [new type name]");
            textWriter.WriteLine(UsageMessageText, typeof(AccountType).GetSingularUIDescription());
        }
    }
}