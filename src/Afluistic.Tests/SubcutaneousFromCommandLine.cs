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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Afluistic.Commands;
using Afluistic.Extensions;
using Afluistic.Services;
using Afluistic.Tests.Extensions;
using Afluistic.Tests.Services;

using FluentAssert;

namespace Afluistic.Tests
{
    public class SubcutaneousFromCommandLine
    {
        private readonly Program _program;
        private readonly InMemorySystemService _systemService;

        public SubcutaneousFromCommandLine()
        {
            _program = IoC.Get<Program>();
            _systemService = IoC.Get<InMemorySystemService>();
        }

        public SubcutaneousFromCommandLine AddAccount(params string[] parameters)
        {
            Execute<AddAccount>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine AddAccountType(params string[] parameters)
        {
            Execute<AddAccountType>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine AddTaxReportingCategory(params string[] parameters)
        {
            Execute<AddTaxReportingCategory>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ChangeAccountName(params string[] parameters)
        {
            Execute<ChangeAccountName>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ChangeAccountType(params string[] parameters)
        {
            Execute<ChangeAccountType>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ChangeAccountTypeName(params string[] parameters)
        {
            Execute<ChangeAccountTypeName>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ChangeAccountTypeTaxabilityType(params string[] parameters)
        {
            Execute<ChangeAccountTypeTaxabilityType>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ClearOutput()
        {
            _systemService.Reset();
            return this;
        }

        private static string[] CreateArgs<T>(IEnumerable<string> parameters)
        {
            return typeof(T).GetTypeNameWords()
                .Select(x => x.ToLower())
                .Concat(parameters)
                .ToArray();
        }

// ReSharper disable MemberCanBeMadeStatic.Global
        public ExecutionArguments CreateExecutionArguments(params string[] parameters)
// ReSharper restore MemberCanBeMadeStatic.Global
        {
            var executionArguments = new ExecutionArguments
                {
                    ApplicationSettings = IoC.Get<ApplicationSettingsService>().Load(),
                    Args = parameters,
                    Statement = IoC.Get<IStorageService>().Load()
                };
            return executionArguments;
        }

        public SubcutaneousFromCommandLine DeleteAccount(params string[] parameters)
        {
            Execute<DeleteAccount>(parameters);
            return this;
        }

        private void Execute<T>(IEnumerable<string> parameters) where T : ICommand
        {
            var args = CreateArgs<T>(parameters);
            _program.Run(args);
        }

        public SubcutaneousFromCommandLine Init(params string[] parameters)
        {
            Execute<Init>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ListAccountTypes(params string[] parameters)
        {
            Execute<ListAccountTypes>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ListAccounts(params string[] parameters)
        {
            Execute<ListAccounts>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ShowAccountType(params string[] parameters)
        {
            Execute<ShowAccountType>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine ShowSettings(params string[] parameters)
        {
            Execute<ShowSettings>(parameters);
            return this;
        }

        public SubcutaneousFromCommandLine VerifyStandardErrorMatches(string messageText)
        {
            Regex.IsMatch(_systemService.StandardErrorText, messageText.ReplaceTypeReferencesWithUIDescriptions(false).MessageTextToRegex()).ShouldBeTrue(_systemService.StandardErrorText);
            return this;
        }

        public SubcutaneousFromCommandLine VerifyStandardOutMatches(string messageText)
        {
            Regex.IsMatch(_systemService.StandardOutText, messageText.ReplaceTypeReferencesWithUIDescriptions(false).MessageTextToRegex()).ShouldBeTrue(_systemService.StandardOutText);
            return this;
        }
    }
}