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

using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class AddAccountType : ICommand
    {
        public const string IncorrectParametersMessageText = "$AccountType name and $TaxabilityType must be specified.";
        public const string InvalidTaxabilityType = "'{0}' is not a valid {1}.";
        public const string NameAlreadyExistsMessageText = "An {0} with that name already exists.";
        public const string SuccessMessageText = "The {0} was added";
        public const string UsageMessageText = "\tAdds an {0}.";
        private readonly IStorageService _storageService;

        public AddAccountType(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [RequireExactlyNArgs(2, IncorrectParametersMessageText)]
        [RequireApplicationSettings]
        [RequireApplicationSettingsAlreadyInitialized]
        [RequireStatement]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;
            if (statement.AccountTypes.Any(x => x.Name == executionArguments.Args[0]))
            {
                return Notification.ErrorFor(NameAlreadyExistsMessageText, typeof(AccountType).GetSingularUIDescription());
            }

            var taxabilityType = TaxabilityType.GetFor(executionArguments.Args[1]);
            if (taxabilityType == null)
            {
                return Notification.ErrorFor(InvalidTaxabilityType, executionArguments.Args[1], typeof(TaxabilityType).GetSingularUIDescription());
            }
            var accountType = new AccountType
                {
                    Name = executionArguments.Args[0],
                    Taxability = taxabilityType
                };
            statement.AccountTypes.Add(accountType);

            var storageResult = _storageService.Save(statement);
            if (storageResult.HasErrors)
            {
                return storageResult;
            }
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