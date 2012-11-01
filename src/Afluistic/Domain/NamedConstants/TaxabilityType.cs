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
    [UIDescription("Taxability type")]
    public class TaxabilityType : NamedConstant<TaxabilityType>
    {
        public static readonly TaxabilityType Taxable = new TaxabilityType("taxed", "Taxable", "Income is taxable at the time it is earned.");
        public static readonly TaxabilityType Taxfree = new TaxabilityType("nontaxed", "Non-taxable", "Income is not taxable while in the account.");

        private TaxabilityType(string key, string label, string description)
        {
            Label = label;
            Description = description;
            Add(key, this);
        }

        public string Description { get; private set; }
        public string Label { get; private set; }
    }
}