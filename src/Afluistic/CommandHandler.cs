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

using System.IO;
using System.Linq;

using Afluistic.Commands;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic
{
    public interface ICommandHandler
    {
        ICommand GetMatchingCommand(string[] args);
        Notification Handle(ICommand command, ExecutionArguments executionArguments);
        void WriteUsage(TextWriter textWriter);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly ICommand[] _commands;
        private readonly IPrerequisiteChecker _prerequisiteChecker;
        private readonly ISystemService _systemService;

        public CommandHandler(IPrerequisiteChecker prerequisiteChecker,
                              ISystemService systemService,
                              ICommand[] commands)
        {
            _commands = commands;
            _prerequisiteChecker = prerequisiteChecker;
            _systemService = systemService;
        }

        public Notification Handle(ICommand command, ExecutionArguments executionArguments)
        {
            var prerequisiteResult = _prerequisiteChecker.Check(command, executionArguments);
            if (prerequisiteResult.HasErrors)
            {
                return prerequisiteResult;
            }
            var commandResult = command.Execute(executionArguments);
            if (commandResult.HasErrors)
            {
                return commandResult;
            }
            if (command.ChangesTheStatement())
            {
                var commandHistory = new CommandHistory
                    {
                        Command = command.GetType().Name,
                        Args = executionArguments.Args,
                        Date = _systemService.CurrentDateTime
                    };
                Statement statement = executionArguments.Statement;
                statement.CommandHistory.Add(commandHistory);
            }
            return commandResult;
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine("Usage:");
            foreach (var command in _commands.OrderBy(x => x.GetType().Name))
            {
                command.WriteUsage(textWriter);
            }
        }

        public ICommand GetMatchingCommand(string[] args)
        {
            return _commands.FirstOrDefault(x => x.IsMatch(args));
        }
    }
}