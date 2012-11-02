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

using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.Prerequisites;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.ArgumentChecks
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class VerifyThatArgument : Attribute, IPrerequisite
    {
        private readonly int _oneBasedArgumentIndex;
        private readonly Type _validationLogicModificationType;
        private readonly Type[] _validators;

        public VerifyThatArgument(int oneBasedArgumentIndex, Type typeOfValidator)
            : this(oneBasedArgumentIndex, typeof(MatchesAllOf), typeOfValidator)
        {
        }

        public VerifyThatArgument(int oneBasedArgumentIndex, Type validationLogicModificationType, params Type[] validators)
        {
            _oneBasedArgumentIndex = oneBasedArgumentIndex;
            _validationLogicModificationType = validationLogicModificationType;
            _validators = validators;
        }

        public Notification Check(ExecutionArguments executionArguments)
        {
            var result = new RequireAtLeastNArgs(_oneBasedArgumentIndex).Check(executionArguments);
            if (result.HasErrors)
            {
                return result;
            }

            var logicModifier = IoC.Get<IArgumentLogicModifier>(_validationLogicModificationType);
            return logicModifier.ApplyTo(executionArguments, _oneBasedArgumentIndex - 1, _validators);
        }

        public int Order
        {
            get { return 10000; }
        }
    }
}