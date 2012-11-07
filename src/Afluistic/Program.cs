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
using System.Linq;

using Afluistic.Commands;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic
{
    public class Program
    {
        public const string DontKnowHowToHandleMessageText = "Don't know how to handle: '{0}'";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly ICommandHandler _commandHandler;
        private readonly IStorageService _storageService;
        private readonly ISystemService _systemService;

        public Program(IApplicationSettingsService applicationSettingsService,
                       IStorageService storageService,
                       ICommandHandler commandHandler,
                       ISystemService systemService)
        {
            _commandHandler = commandHandler;
            _systemService = systemService;
            _applicationSettingsService = applicationSettingsService;
            _storageService = storageService;
        }

        private Notification Handle(string[] args)
        {
            if (args.Length == 0)
            {
                _commandHandler.WriteUsage(_systemService.StandardOut);
                return Notification.Empty;
            }

            var command = _commandHandler.GetMatchingCommand(args);
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

            var result = _commandHandler.Handle(command, executionArguments);
            if (result.HasErrors)
            {
                return result;
            }

            if (command.ChangesTheApplicationSettings())
            {
                var saveResult = _applicationSettingsService.Save(executionArguments.ApplicationSettings);
                if (saveResult.HasErrors)
                {
                    return saveResult;
                }
            }

            if (command.ChangesTheStatement())
            {
                var saveResult = _storageService.Save(executionArguments.Statement);
                if (saveResult.HasErrors)
                {
                    return saveResult;
                }
            }
            return result;
        }

        private static void Main(string[] args)
        {
            IoC.Initialize();

            var program = IoC.Get<Program>();
            program.Run(args);
        }

        public void Run(string[] args)
        {
            var result = Handle(args);
            WriteResults(result);
        }

        private void WriteResults(Notification result)
        {
            if (result.HasErrors)
            {
                _systemService.StandardError.WriteLine(result.Errors);
            }
            else
            {
                _systemService.StandardOut.WriteLine(result.Infos);
            }
        }
    }
}