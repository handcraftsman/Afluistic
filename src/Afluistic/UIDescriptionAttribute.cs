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

using Afluistic.Extensions;

namespace Afluistic
{
    public class UIDescriptionAttribute : Attribute
    {
        public UIDescriptionAttribute(string uiDescription)
            : this(uiDescription, uiDescription.Pluralize())
        {
        }

        public UIDescriptionAttribute(string uiDescription, string pluralUiDescription)
        {
            UIDescription = uiDescription.ReplaceTypeReferencesWithUIDescriptions(false);
            PluralUIDescription = pluralUiDescription.ReplaceTypeReferencesWithUIDescriptions(true);
        }

        public string PluralUIDescription { get; private set; }
        public string UIDescription { get; private set; }
    }
}