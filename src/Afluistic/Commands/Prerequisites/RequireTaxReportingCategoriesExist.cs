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

using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands.Prerequisites
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireTaxReportingCategoriesExist : Attribute, IPrerequisite
    {
        public const string NoTaxReportingCategoriesMessageText = "There are no {0}.";

        public Notification Check(ExecutionArguments executionArguments)
        {
            Statement statement = executionArguments.Statement;
            if (!statement.TaxReportingCategories.Any())
            {
                return Notification.ErrorFor(NoTaxReportingCategoriesMessageText, typeof(TaxReportingCategory).GetPluralUIDescription());
            }

            return Notification.Empty;
        }

        public int Order
        {
            get { return 600; }
        }
    }
}