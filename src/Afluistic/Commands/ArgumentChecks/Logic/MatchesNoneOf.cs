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
using System.Collections.Generic;
using System.Linq;

using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.ArgumentChecks.Logic
{
    public class MatchesNoneOf : IArgumentLogicModifier
    {
        public const string ErrorMessageText = "Unexpectedly matched: {0}";
        private readonly IArgumentValidator[] _validators;

        public MatchesNoneOf(IArgumentValidator[] validators)
        {
            _validators = validators;
        }

        public Notification ApplyTo(ExecutionArguments executionArguments, int argumentIndex, IList<Type> argumentValidatorTypes)
        {
            var requestedValidators = _validators.Where(x => argumentValidatorTypes.Contains(x.GetType()));

            foreach (var argumentValidator in requestedValidators)
            {
                var result = argumentValidator.Check(executionArguments, argumentIndex);
                if (!result.HasErrors)
                {
                    return Notification.ErrorFor(ErrorMessageText, argumentValidator.GetType().GetSingularUIDescription());
                }
            }
            return Notification.Empty;
        }
    }
}