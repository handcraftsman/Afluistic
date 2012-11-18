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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Afluistic.Services
{
    public interface IJsonSerializer
    {
        T DeserializeFromFile<T>(string path);
        void SerializeToFile<T>(string path, T obj);
    }

    public class JsonSerializer : IJsonSerializer
    {
        private readonly IFileSystemService _fileSystemService;

        public JsonSerializer(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        public T DeserializeFromFile<T>(string path)
        {
            var serializer = Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings());
            serializer.Converters.Add(new NamedConstantJsonConverter());
            using (var reader = _fileSystemService.GetStreamReader(path))
            {
                var content = (T)serializer.Deserialize(reader, typeof(T));
                return content;
            }
        }

        public void SerializeToFile<T>(string path, T obj)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                };
            serializer.Converters.Add(new IsoDateTimeConverter());
            serializer.Converters.Add(new NamedConstantJsonConverter());

            using (var streamWriter = _fileSystemService.GetStreamWriter(path))
            {
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    using (var writer = jsonTextWriter)
                    {
                        serializer.Serialize(writer, obj);
                    }
                }
            }
        }
    }
}