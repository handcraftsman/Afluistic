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

using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.Prerequisites
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireExactlyNArgs : Attribute, IPrerequisite
    {
        public const string WrongNumberOfArgumentsMessageText = "Wrong number arguments specified - please check the usage.";

        private readonly int _expectedNumberOfAdditionalArgs;
        private readonly string _messageText;

        public RequireExactlyNArgs(int expectedNumberOfAdditionalArgs)
            : this(expectedNumberOfAdditionalArgs, WrongNumberOfArgumentsMessageText)
        {
        }

        public RequireExactlyNArgs(int expectedNumberOfAdditionalArgs, string messageText)
        {
            _expectedNumberOfAdditionalArgs = expectedNumberOfAdditionalArgs;
            _messageText = messageText.ReplaceTypeReferencesWithUIDescriptions(false);
        }

        public Notification Check(ExecutionArguments executionArguments)
        {
            var args = executionArguments.Args;
            if (args.Length != _expectedNumberOfAdditionalArgs)
            {
                return Notification.ErrorFor(_messageText);
            }
            return Notification.Empty;
        }

        public int Order
        {
            get { return 100; }
        }
    }
}