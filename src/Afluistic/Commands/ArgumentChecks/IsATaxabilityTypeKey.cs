﻿// * **************************************************************************
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
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.ArgumentChecks
{
    public class IsATaxabilityTypeKey : IArgumentValidator
    {
        public const string InvalidTaxabilityType = "'{0}' is not a valid {1}.";

        public Notification Check(ExecutionArguments executionArguments, int argumentIndex)
        {
            var argument = executionArguments.Args[argumentIndex];

            var taxabilityType = TaxabilityType.GetFor(argument);
            if (taxabilityType == null)
            {
                return Notification.ErrorFor(InvalidTaxabilityType, executionArguments.Args[1], typeof(TaxabilityType).GetSingularUIDescription());
            }

            return Notification.Empty;
        }
    }
}