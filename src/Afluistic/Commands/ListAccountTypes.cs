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

using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class ListAccountTypes : ICommand
    {
        public const string UsageMessageText = "\tLists the supported {0}.";
        private readonly ISystemService _systemService;

        public ListAccountTypes(ISystemService systemService)
        {
            _systemService = systemService;
        }

        [RequireExactlyNArgs(0)]
        [RequireStatement]
        [RequireAccountTypesExist]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;

            foreach (var indexed in statement.AccountTypes.GetIndexedValues())
            {
                _systemService.StandardOut.WriteLine(indexed.ToString(x => x.Name));
            }

            return Notification.Empty;
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()));
            textWriter.WriteLine(UsageMessageText, typeof(AccountType).GetPluralUIDescription());
        }
    }
}