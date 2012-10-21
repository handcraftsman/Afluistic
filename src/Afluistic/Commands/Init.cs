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

using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class Init : ICommand
    {
        public const string FilePathNotSpecifiedMessageText = "filepath must be specified.";
        public const string SuccessMessageText = "{0} was created at {1}";
        public const string TooManyArgumentsMessageText = "too many arguments specified - please check the usage.";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IStorageService _storageService;
        private readonly ISystemService _systemService;

        public Init(IApplicationSettingsService applicationSettingsService,
                    IStorageService storageService,
                    ISystemService systemService)
        {
            _applicationSettingsService = applicationSettingsService;
            _storageService = storageService;
            _systemService = systemService;
        }

        public Notification Execute(string[] args)
        {
            var commandWords = this.GetCommandWords();
            if (args.Length < 1 + commandWords.Length)
            {
                return Notification.ErrorFor(FilePathNotSpecifiedMessageText);
            }
            if (args.Length > 1 + commandWords.Length)
            {
                return Notification.ErrorFor(TooManyArgumentsMessageText);
            }

            var settingsResult = _applicationSettingsService.Load();
            if (settingsResult.HasErrors)
            {
                return settingsResult;
            }
            if (settingsResult.HasWarnings)
            {
                _systemService.StandardOut.WriteLine(settingsResult.Warnings);
            }

            ApplicationSettings applicationSettings = settingsResult;
            applicationSettings.StatementPath = args[commandWords.Length];

            var saveResult = _applicationSettingsService.Save(applicationSettings);
            if (saveResult.HasErrors)
            {
                return saveResult;
            }

            var storageResult = _storageService.Save(new Statement());
            if (storageResult.HasErrors)
            {
                return storageResult;
            }
            return Notification.InfoFor(SuccessMessageText, typeof(Statement).GetUIDescription(), applicationSettings.StatementPath);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [filepath]");
            textWriter.WriteLine("\tInitializes the default {0} at [filepath].", typeof(Statement).GetUIDescription());
        }
    }
}