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

using Afluistic.Extensions;

namespace Afluistic
{
    public class UIDescriptionAttribute : Attribute
    {
        public UIDescriptionAttribute(string uiDescription)
        {
            var indexOfMarker = uiDescription.IndexOf('$');
            if (indexOfMarker != -1)
            {
                var end = uiDescription.IndexOf(' ', indexOfMarker);
                var typeName = uiDescription.Substring(1 + indexOfMarker, end - indexOfMarker - 1);
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.Name == typeName);
                var typeDescription = type.GetUIDescription();
                uiDescription = uiDescription.Substring(0, indexOfMarker) + typeDescription + uiDescription.Substring(end);
            }
            UIDescription = uiDescription;
        }

        public string UIDescription { get; private set; }
    }
}