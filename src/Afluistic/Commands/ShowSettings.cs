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
    public class ShowSettings : ICommand
    {
        public const string TooManyArgumentsMessageText = "too many arguments specified - please check the usage.";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly ISystemService _systemService;

        public ShowSettings(IApplicationSettingsService applicationSettingsService,
                            ISystemService systemService)
        {
            _applicationSettingsService = applicationSettingsService;
            _systemService = systemService;
        }

        public Notification Execute(string[] args)
        {
            var commandWords = this.GetCommandWords();
            if (args.Length != commandWords.Length)
            {
                return Notification.ErrorFor(TooManyArgumentsMessageText);
            }

            var settingsResult = _applicationSettingsService.Load();
            if (settingsResult.HasErrors || settingsResult.HasWarnings)
            {
                return settingsResult;
            }

            ApplicationSettings applicationSettings = settingsResult;
            _systemService.StandardOut.WriteLine(TypeExtensions.GetUIDescription<ApplicationSettings>(x => x.StatementPath) + ":\t" + applicationSettings.StatementPath);
            return Notification.Empty;
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()));
            textWriter.WriteLine("\tShows the {0}.", typeof(ApplicationSettings).GetUIDescription());
        }
    }
}