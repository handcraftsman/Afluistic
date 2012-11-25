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
using System.Collections.Generic;

namespace Afluistic.Domain
{
    public class Statement
    {
        public Statement()
        {
            Accounts = new List<Account>();
            AccountTypes = new List<AccountType>();
            CommandHistory = new List<CommandHistory>();
            TaxReportingCategories = new List<TaxReportingCategory>();
            TransactionTypes = new List<TransactionType>();
        }

        public IList<AccountType> AccountTypes { get; set; }
        public IList<Account> Accounts { get; set; }
        public IList<CommandHistory> CommandHistory { get; set; }
        public Account SelectedAccount { get; set; }
        public IList<TaxReportingCategory> TaxReportingCategories { get; set; }
        public IList<TransactionType> TransactionTypes { get; set; }
    }
}