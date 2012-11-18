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
using System.IO;
using System.Linq;

using Afluistic.Commands.ArgumentChecks;
using Afluistic.Commands.PostConditions;
using Afluistic.Commands.Prerequisites;
using Afluistic.Domain;
using Afluistic.Domain.NamedConstants;
using Afluistic.Extensions;
using Afluistic.MvbaCore;

namespace Afluistic.Commands
{
    public class Init : ICommand, IChangeApplicationSettings, IChangeStatement
    {
        public const string FilePathNotSpecifiedMessageText = "filepath must be specified.";
        public const string SuccessMessageText = "The {0} was created at {1}";
        public const string UsageMessageText = "\tInitializes a {0} at [filepath].";

        [RequireExactlyNArgs(1, FilePathNotSpecifiedMessageText)]
        [RequireApplicationSettings]
        [VerifyThatArgument(1, typeof(IsAFilePath))]
        public Notification Execute(ExecutionArguments executionArguments)
        {
            ApplicationSettings applicationSettings = executionArguments.ApplicationSettings;
            applicationSettings.StatementPath = executionArguments.Args.Last().ToStatementPath();
            var statement = new Statement();
            AddDefaultAccountTypes(statement);
            AddDefaultTaxReportingCategories(statement);
            AddDefaultTransactionTypes(statement);
            executionArguments.Statement = statement;

            return Notification.InfoFor(SuccessMessageText, typeof(Statement).GetSingularUIDescription(), applicationSettings.StatementPath);
        }

        public void WriteUsage(TextWriter textWriter)
        {
            textWriter.WriteLine(String.Join(" ", this.GetCommandWords()) + " [filepath]");
            textWriter.WriteLine(UsageMessageText, typeof(Statement).GetSingularUIDescription());
        }

        private static void AddDefaultAccountTypes(Statement statement)
        {
            foreach (var accountType in GetDefaultAccountTypes())
            {
                statement.AccountTypes.Add(accountType);
            }
        }

        private static void AddDefaultTaxReportingCategories(Statement statement)
        {
            foreach (var category in GetDefaultTaxReportingCategories())
            {
                statement.TaxReportingCategories.Add(category);
            }
        }

        private static void AddDefaultTransactionTypes(Statement statement)
        {
            foreach (var transactionType in GetDefaultTransactionTypes())
            {
                statement.TransactionTypes.Add(transactionType);
            }
        }

        public static IEnumerable<AccountType> GetDefaultAccountTypes()
        {
            var accountTypes = new[]
                {
                    new AccountType
                        {
                            Name = "Checking",
                            Taxability = TaxabilityType.Taxable
                        },
                    new AccountType
                        {
                            Name = "Savings",
                            Taxability = TaxabilityType.Taxable
                        },
                    new AccountType
                        {
                            Name = "Brokerage",
                            Taxability = TaxabilityType.Taxable
                        },
                    new AccountType
                        {
                            Name = "401k",
                            Taxability = TaxabilityType.Taxfree
                        },
                    new AccountType
                        {
                            Name = "IRA",
                            Taxability = TaxabilityType.Taxfree
                        },
                    new AccountType
                        {
                            Name = "Roth IRA",
                            Taxability = TaxabilityType.Taxfree
                        }
                };
            return accountTypes;
        }

        public static IEnumerable<TaxReportingCategory> GetDefaultTaxReportingCategories()
        {
            var categories = new[]
                {
                    new TaxReportingCategory
                        {
                            Name = "N/A"
                        },
                    new TaxReportingCategory
                        {
                            Name = "Dividend"
                        },
                    new TaxReportingCategory
                        {
                            Name = "Interest"
                        },
                    new TaxReportingCategory
                        {
                            Name = "Long-Term Capital Gain"
                        },
                    new TaxReportingCategory
                        {
                            Name = "Short-Term Capital Gain"
                        },
                    new TaxReportingCategory
                        {
                            Name = "To be determined"
                        },
                };
            return categories;
        }

        public static IEnumerable<TransactionType> GetDefaultTransactionTypes()
        {
            var taxReportingCategoryLookup = GetDefaultTaxReportingCategories().ToDictionary(x => x.Name);

            var transactionType = new[]
                {
                    new TransactionType
                        {
                            Name = "Credit",
                            Category = taxReportingCategoryLookup["N/A"]
                        },
                    new TransactionType
                        {
                            Name = "Debit",
                            Category = taxReportingCategoryLookup["N/A"]
                        },
                    new TransactionType
                        {
                            Name = "Buy",
                            Category = taxReportingCategoryLookup["N/A"]
                        },
                    new TransactionType
                        {
                            Name = "Sell",
                            Category = taxReportingCategoryLookup["To be determined"]
                        },
                    new TransactionType
                        {
                            Name = "Reinvest Dividend",
                            Category = taxReportingCategoryLookup["Dividend"]
                        },
                    new TransactionType
                        {
                            Name = "Reinvest Short-Term Capital Gain",
                            Category = taxReportingCategoryLookup["Short-Term Capital Gain"]
                        },
                    new TransactionType
                        {
                            Name = "Reinvest Long-Term Capital Gain",
                            Category = taxReportingCategoryLookup["Long-Term Capital Gain"]
                        },
                    new TransactionType
                        {
                            Name = "Add Shares",
                            Category = taxReportingCategoryLookup["N/A"]
                        },
                    new TransactionType
                        {
                            Name = "Remove Shares",
                            Category = taxReportingCategoryLookup["N/A"]
                        },
                };
            return transactionType;
        }
    }
}