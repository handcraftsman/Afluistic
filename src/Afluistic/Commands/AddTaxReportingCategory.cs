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

using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.ArgumentChecks.Logic;
using Afluistic.Commands.PostConditions;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands
{
    public class AddTaxReportingCategory : ICommand, IChangeStatement
    {
        public const string IncorrectParametersMessageText = "$TaxReportingCategory name must be specified.";
        public const string SuccessMessageText = "The {0} was added";
        public const string UsageMessageText = "\tAdds a {0}.";

        [RequireStatement]
        [RequireExactlyNArgs(1, IncorrectParametersMessageText)]
        [VerifyThatArgument(1, typeof(MatchesNoneOf), typeof(IsTheNameOfAnExistingTaxReportingCategory))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            var category = new TaxReportingCategory
                {
                    Name = executionArguments.Args[0],
                };

            Statement statement = executionArguments.Statement;
            statement.TaxReportingCategories.Add(category);

            return Notification.InfoFor(SuccessMessageText, typeof(TaxReportingCategory).GetSingularUIDescription());
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [new category name]");
            textWriter.WriteLine(UsageMessageText, typeof(TaxReportingCategory).GetSingularUIDescription());
        }
    }
}