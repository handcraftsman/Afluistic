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
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class ChangeAccountTypeTaxabilityType : ICommand
    {
        public const string IncorrectParametersMessageText = "$AccountType name or index and a $TaxabilityType must be specified.";
        public const string SuccessMessageText = "The {0} was changed";
        public const string UsageMessageText = "\tChanges the {0} of an {1}.";
        private readonly IStorageService _storageService;

        public ChangeAccountTypeTaxabilityType(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireStatement]
        [VerifyThatArgument(1, typeof(MatchesAnyOf), typeof(IsTheNameOfAnExistingAccountType), typeof(IsTheIndexOfAnExistingAccountType))]
        [VerifyThatArgument(2, typeof(IsATaxabilityTypeKey))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            var accountType = statement.AccountTypes.GetByPropertyValueOrIndex(x => x.Name, executionArguments.Args[0]);
            accountType.Taxability = TaxabilityType.GetFor(executionArguments.Args[1]);

            var storageResult = _storageService.Save(statement);
            if (storageResult.HasErrors)
            {
                return storageResult;
            }
            return Notification.InfoFor(SuccessMessageText, typeof(TaxabilityType).GetSingularUIDescription());
        }

        public void WriteUsage(TextWriter textWriter)
        {
            var taxabilityKeys = String.Join("|", TaxabilityType.GetAll().Select(x => x.Key).ToArray());
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [type name|index#] [" + taxabilityKeys + "]");
            textWriter.WriteLine(UsageMessageText, typeof(TaxabilityType).GetSingularUIDescription(), typeof(AccountType).GetSingularUIDescription());
        }
    }
}