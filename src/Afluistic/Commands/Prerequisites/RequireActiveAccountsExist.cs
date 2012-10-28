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
using System.Linq;

using Afluistic.Domain;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.Prerequisites
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireActiveAccountsExist : Attribute, IPrerequisite
    {
        public const string NoActiveAccountsMessageText = "There are no active accounts.";

        public Notification Check(ExecutionArguments exectionArguments)
        {
            Statement statement = exectionArguments.Statement;
            if (!statement.Accounts.Any(x => !x.IsDeleted))
            {
                return Notification.ErrorFor(NoActiveAccountsMessageText);
            }

            return Notification.Empty;
        }

        public int Order
        {
            get { return 500; }
        }
    }
}