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
using System.Collections.Generic;

using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Services
{
    public interface IStorageService
    {
        Notification<Statement> Load();
        Notification Save(Statement statement);
    }

    public class StorageService : IStorageService
    {
        public const string DeserializationErrorMessageText = "Unable to load the {0} to {1} due to{2}{3}";
        public const string InitializationErrorMessageText = "Please initialize the {0} filepath.";
        public const string SerializationErrorMessageText = "Unable to save the {0} to {1} due to{2}{3}";
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly ISerializationService _serializationService;

        public StorageService(ISerializationService serializationService,
                              IApplicationSettingsService applicationSettingsService)
        {
            _serializationService = serializationService;
            _applicationSettingsService = applicationSettingsService;
        }

        public Notification Save(Statement statement)
        {
            var settingsResult = _applicationSettingsService.Load();
            if (settingsResult.HasErrors)
            {
                return settingsResult;
            }

            ApplicationSettings settings = settingsResult;
            var path = settings.StatementPath;
            if (path.IsNullOrEmpty())
            {
                return Notification.ErrorFor(InitializationErrorMessageText, typeof(Statement).GetSingularUIDescription());
            }
            var result = _serializationService.SerializeToFile(statement, path);
            if (result.HasErrors)
            {
                return Notification.ErrorFor(SerializationErrorMessageText, typeof(Statement).GetSingularUIDescription(), path, Environment.NewLine, result.Errors);
            }
            return result;
        }

        public Notification<Statement> Load()
        {
            var settingsResult = _applicationSettingsService.Load();
            if (settingsResult.HasErrors)
            {
                return settingsResult.ToNotification<Statement>();
            }

            ApplicationSettings settings = settingsResult;
            var path = settings.StatementPath;
            if (path.IsNullOrEmpty())
            {
                return Notification.ErrorFor(InitializationErrorMessageText, typeof(Statement).GetSingularUIDescription()).ToNotification<Statement>();
            }

            var statement = _serializationService.DeserializeFromFile<Statement>(path);
            if (statement.HasErrors)
            {
                return Notification.ErrorFor(DeserializationErrorMessageText, typeof(Statement).GetSingularUIDescription(), path, Environment.NewLine, statement.Errors).ToNotification<Statement>();
            }
            return statement;
        }
    }
}