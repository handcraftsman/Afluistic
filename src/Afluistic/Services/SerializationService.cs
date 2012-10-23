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

using Afluistic.MvbaCore;

namespace Afluistic.Services
{
    public interface ISerializationService
    {
        Notification<T> DeserializeFromFile<T>(string settingsPath);
        Notification SerializeToFile<T>(T obj, string path);
    }

    public class SerializationService : ISerializationService
    {
        public const string SerializationErrorMessageText = "Unable to serialize object to {0} due to{1}{2}";
        public const string DeserializationErrorMessageText = "Unable to deserialize object from {0} due to{1}{2}";
        private readonly IJsonSerializer _jsonSerializer;

        public SerializationService(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public Notification SerializeToFile<T>(T obj, string path)
        {
            try
            {
                _jsonSerializer.SerializeToFile(path, obj);
                return Notification.Empty;
            }
            catch (Exception e)
            {
                return Notification.ErrorFor(SerializationErrorMessageText, path, Environment.NewLine, e.Message);
            }
        }

        public Notification<T> DeserializeFromFile<T>(string path)
        {
            try
            {
                var obj = _jsonSerializer.DeserializeFromFile<T>(path);
                return obj;
            }
            catch (Exception e)
            {
                return Notification.ErrorFor(DeserializationErrorMessageText, path, Environment.NewLine, e.Message).ToNotification<T>();
            }
        }
    }
}