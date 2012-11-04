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
using System.Linq.Expressions;

using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;
using Afluistic.Services;

namespace Afluistic.Commands
{
    public class ShowSettings : ICommand
    {
        public const string UsageMessageText = "\tShows the {0}.";
        private readonly ISystemService _systemService;

        public ShowSettings(ISystemService systemService)
        {
            _systemService = systemService;
        }

        [RequireExactlyNArgs(0)]
        [RequireApplicationSettings]
        [RequireApplicationSettingsAlreadyInitialized]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            ApplicationSettings applicationSettings = executionArguments.ApplicationSettings;
            _systemService.StandardOut.WriteLine(GetLabelFor(x => x.StatementPath) + ":\t" + applicationSettings.StatementPath);
            return Notification.Empty;
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()));
            textWriter.WriteLine(UsageMessageText, typeof(ApplicationSettings).GetSingularUIDescription());
        }

        private static string GetLabelFor(Expression<Func<ApplicationSettings, object>> func)
        {
            return TypeExtensions.GetSingularUIDescription(func);
        }
    }
}