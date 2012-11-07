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
using Afluistic.Commands.PostConditions;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands
{
    public class AddAccountType : ICommand, IChangeStatement
    {
        public const string IncorrectParametersMessageText = "$AccountType name and $TaxabilityType must be specified.";
        public const string SuccessMessageText = "The {0} was added";
        public const string UsageMessageText = "\tAdds an {0}.";

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesNoneOf), typeof(IsTheNameOfAnExistingAccountType))]
        [VerifyThatArgument(2, typeof(IsATaxabilityTypeKey))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            var accountType = new AccountType
                {
                    Name = executionArguments.Args[0],
                    Taxability = TaxabilityType.GetFor(executionArguments.Args[1])
                };

            Statement statement = executionArguments.Statement;
            statement.AccountTypes.Add(accountType);

            return Notification.InfoFor(SuccessMessageText, typeof(AccountType).GetSingularUIDescription());
        }

        public void WriteUsage(TextWriter textWriter)
        {
            var taxabilityKeys = String.Join("|", TaxabilityType.GetAll().Select(x => x.Key).ToArray());
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [new type name] [" + taxabilityKeys + "]");
            textWriter.WriteLine(UsageMessageText, typeof(AccountType).GetSingularUIDescription());
        }
    }
}