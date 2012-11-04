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
using System.Linq.Expressions;

using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class ShowAccountType : ICommand
    {
        public const string IncorrectParametersMessageText = "An $AccountType name or index must be specified.";
        public const string UsageMessageText = "\tShows the details of a partcular {0}.";
        private readonly ISystemService _systemService;

        public ShowAccountType(ISystemService systemService)
        {
            _systemService = systemService;
        }

        [RequireExactlyNArgs(1, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccountType), typeof(IsTheIndexOfAnExistingAccountType))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var accountType = statement.AccountTypes.GetIndexedValues()
                .First(x => x.Item.Name == executionArguments.Args[0] || x.Index.ToString() == executionArguments.Args[0]).Item;

            _systemService.StandardOut.WriteLine(GetLabelFor(x => x.Name) + ":\t" + accountType.Name);
            _systemService.StandardOut.WriteLine(GetLabelFor(x => x.Taxability) + ":\t" + accountType.Taxability.Label);

            return Notification.Empty;
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [type name|index#]");
            textWriter.WriteLine(UsageMessageText, typeof(AccountType).GetSingularUIDescription());
        }

        private static string GetLabelFor(Expression<Func<AccountType, object>> func)
        {
            return TypeExtensions.GetSingularUIDescription(func);
        }
    }
}