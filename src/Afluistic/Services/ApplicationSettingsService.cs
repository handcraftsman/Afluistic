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

namespace Afluistic.Services
{
    public interface IApplicationSettingsService
    {
        Notification<ApplicationSettings> Load();
        Notification Save(ApplicationSettings applicationSettings);
    }

    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public const string DamagedSettingsFileMessageText = "Failed to load {0} from {1} due to{2}{3}";
        public const string MissingSettingsFileMessageText = "Failed to load {0} from {2} because the file does not exist. The {1} filepath needs to be initialized.";
        private readonly IFileSystemService _fileSystemService;
        private readonly ISerializationService _serializationService;
        private readonly ISystemService _systemService;

        public ApplicationSettingsService(ISystemService systemService,
                                          IFileSystemService fileSystemService,
                                          ISerializationService serializationService)
        {
            _systemService = systemService;
            _fileSystemService = fileSystemService;
            _serializationService = serializationService;
        }

        public Notification<ApplicationSettings> Load()
        {
            var settingsPath = GetSettingsPath();
            var settings = _serializationService.DeserializeFromFile<ApplicationSettings>(settingsPath);
            if (settings.HasErrors)
            {
                if (_fileSystemService.FileExists(settingsPath))
                {
                    return Notification.ErrorFor(DamagedSettingsFileMessageText, typeof(ApplicationSettings).GetUIDescription(), settingsPath, Environment.NewLine, settings.Errors).ToNotification<ApplicationSettings>();
                }
                return Notification.WarningFor(MissingSettingsFileMessageText, typeof(ApplicationSettings).GetUIDescription(), typeof(Statement).GetUIDescription(), settingsPath).ToNotification(new ApplicationSettings());
            }
            return settings;
        }

        public Notification Save(ApplicationSettings applicationSettings)
        {
            return _serializationService.SerializeToFile(applicationSettings, GetSettingsPath());
        }

        private string GetSettingsPath()
        {
            return Path.Combine(_systemService.AppDataDirectory, Constants.Settings.FileName);
        }
    }
}