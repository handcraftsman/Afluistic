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
using System.Reflection;

using Afluistic.MvbaCore;

using Newtonsoft.Json;

using QIFGet.MvbaCore.NamedConstants;

namespace Afluistic.Services
{
    public class NamedConstantJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(NamedConstant).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {

            var nc = typeof(NamedConstant<>).MakeGenericType(objectType);
            var getFor = nc.GetMethod("GetFor", BindingFlags.Static|BindingFlags.Public);
            var result = getFor.Invoke(nc, new[]{reader.Value});
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var namedConstant = (NamedConstant)value;
            serializer.Serialize(writer, namedConstant.Key);
        }
    }
}