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
using QIFGet.MvbaCore.NamedConstants;

namespace Afluistic.Domain.NamedConstants
{
    [UIDescription("Transaction detail type")]
    public class TransactionDetailType : NamedConstant<TransactionDetailType>
    {
        public static readonly TransactionDetailType CashAdded = new TransactionDetailType("+cash", "Add cash");
        public static readonly TransactionDetailType CashRemoved = new TransactionDetailType("-cash", "Remove cash");
        public static readonly TransactionDetailType CommentOptional = new TransactionDetailType("comment?", "Optional Comment/Memo");
        public static readonly TransactionDetailType Date = new TransactionDetailType("date", "Date");
        public static readonly TransactionDetailType Fees = new TransactionDetailType("fees", "Fees");
        public static readonly TransactionDetailType SharesAdded = new TransactionDetailType("+shares", "Add Shares");
        public static readonly TransactionDetailType SharesRemoved = new TransactionDetailType("-shares", "Remove Shares");
        public static readonly TransactionDetailType Symbol = new TransactionDetailType("symbol", "Security Symbol");
        public static readonly TransactionDetailType Total = new TransactionDetailType("total", "Total including fees");

        private TransactionDetailType(string key, string description)
        {
            Description = description;
            Add(key, this);
        }

        public string Description { get; private set; }
    }
}