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

using Afluistic.Commands;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic
{
    public interface ICommandHandler
    {
        Notification Handle(string[] args);
        void WriteUsage(TextWriter textWriter);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly ICommand[] _commands;

        public CommandHandler(ICommand[] commands)
        {
            _commands = commands;
        }

        public Notification Handle(string[] args)
        {
            var command = GetMatchingCommand(args);
            if (command == null)
            {
                return Notification.ErrorFor("Don't know how to handle: '" + String.Join(" ", args) + "'");
            }
            return command.Execute(args);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine("Usage:");
            foreach (var command in _commands.OrderBy(x => x.GetType().Name))
            {
                command.WriteUsage(textWriter);
            }
        }

        private ICommand GetMatchingCommand(string[] args)
        {
            return _commands.FirstOrDefault(x => x.IsMatch(args));
        }
    }
}