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
using System.Linq;

using Afluistic.Commands;
using Afluistic.Commands.PostConditions;
using Afluistic.Commands.Prerequisites;
using Afluistic.MvbaCore;

namespace Afluistic.Extensions
{
    public static class CommandExtensions
    {
        private static readonly IDictionary<Type, IPrerequisite[]> CachedPrerequisites = new Dictionary<Type, IPrerequisite[]>();

        public static bool ChangesTheApplicationSettings(this ICommand command)
        {
            return command is IChangeApplicationSettings;
        }

        public static bool ChangesTheStatement(this ICommand command)
        {
            return command is IChangeStatement;
        }

        public static IEnumerable<IPrerequisite> GetCommandExecutionPrerequisites(this ICommand command)
        {
            IPrerequisite[] prerequisites;
            if (!CachedPrerequisites.TryGetValue(command.GetType(), out prerequisites))
            {
                prerequisites = command.GetType()
                    .GetMethod(Reflection.GetMethodName((ICommand c) => c.Execute(null)))
                    .GetCustomAttributes(typeof(IPrerequisite), false)
                    .Cast<IPrerequisite>()
                    .ToArray();
                CachedPrerequisites.Add(command.GetType(), prerequisites);
            }
            return prerequisites;
        }

        public static string[] GetCommandWords(this ICommand command)
        {
            var commandWords = command.GetType()
                .GetTypeNameWords()
                .Select(x => x.ToLower())
                .ToArray();
            return commandWords;
        }

        public static bool IsMatch(this ICommand command, string[] args)
        {
            var commandWords = command.GetCommandWords();
            if (args.Length < commandWords.Length)
            {
                return false;
            }
            var isMatch = commandWords
                .Select((x, i) => new
                    {
                        command = x,
                        arg = args[i]
                    })
                .All(x => x.command == x.arg);
            return isMatch;
        }
    }
}