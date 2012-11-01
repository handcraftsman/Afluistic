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

using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class Init : ICommand
    {
        public const string FilePathNotSpecifiedMessageText = "filepath must be specified.";
        public const string SuccessMessageText = "The {0} was created at {1}";
        public const string UsageMessageText = "\tInitializes a {0} at [filepath].";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IStorageService _storageService;

        public Init(IApplicationSettingsService applicationSettingsService,
                    IStorageService storageService)
        {
            _applicationSettingsService = applicationSettingsService;
            _storageService = storageService;
        }

        [RequireExactlyNArgs(1, FilePathNotSpecifiedMessageText)]
        [RequireApplicationSettings]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            ApplicationSettings applicationSettings = executionArguments.ApplicationSettings;
            var statementPath = executionArguments.Args.Last().ToStatementPath();
            if (statementPath.HasErrors)
            {
                return statementPath;
            }
            applicationSettings.StatementPath = statementPath;

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
            return Notification.InfoFor(SuccessMessageText, typeof(Statement).GetSingularUIDescription(), applicationSettings.StatementPath);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [filepath]");
            textWriter.WriteLine(UsageMessageText, typeof(Statement).GetSingularUIDescription());
        }
    }
}