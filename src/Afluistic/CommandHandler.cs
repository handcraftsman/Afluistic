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
using Afluistic.Commands.Prerequisites;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic
{
    public interface ICommandHandler
    {
        Notification Handle(string[] args);
        void WriteUsage(TextWriter textWriter);
    }

    public class CommandHandler : ICommandHandler
    {
        public const string DontKnowHowToHandleMessageText = "Don't know how to handle: '{0}'";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly ICommand[] _commands;
        private readonly IPrerequisiteChecker _prerequisiteChecker;
        private readonly IStorageService _storageService;

        public CommandHandler(IApplicationSettingsService applicationSettingsService,
                              IPrerequisiteChecker prerequisiteChecker,
                              IStorageService storageService,
                              ICommand[] commands)
        {
            _commands = commands;
            _applicationSettingsService = applicationSettingsService;
            _storageService = storageService;
            _prerequisiteChecker = prerequisiteChecker;
        }

        public Notification Handle(string[] args)
        {
            var command = GetMatchingCommand(args);
            if (command == null)
            {
                return Notification.ErrorFor(DontKnowHowToHandleMessageText, String.Join(" ", args));
            }
            var executionArguments = new ExecutionArguments
                {
                    Args = args.Skip(command.GetCommandWords().Length).ToArray(),
                    ApplicationSettings = _applicationSettingsService.Load(),
                    Statement = _storageService.Load()
                };
            var prerequisiteResult = _prerequisiteChecker.Check(command, executionArguments);
            if (prerequisiteResult.HasErrors)
            {
                return prerequisiteResult;
            }
            return command.Execute(executionArguments);
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