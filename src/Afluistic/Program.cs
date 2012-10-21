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
using Afluistic.Services;

namespace Afluistic
{
    public class Program
    {
        private readonly ICommandHandler _commandHandler;
        private readonly ISystemService _systemService;

        public Program(
            ICommandHandler commandHandler,
            ISystemService systemService)
        {
            _commandHandler = commandHandler;
            _systemService = systemService;
        }

        public void Handle(string[] args)
        {
            if (args.Length == 0)
            {
                _commandHandler.WriteUsage(_systemService.StandardOut);
                return;
            }

            var result = _commandHandler.Handle(args);
            if (result.HasErrors)
            {
                _systemService.StandardError.WriteLine(result.Errors);
            }
            else
            {
                _systemService.StandardOut.WriteLine(result.Infos);
            }
        }

        private static void Main(string[] args)
        {
            IoC.Initialize();

            IoC.Get<Program>().Handle(args);
        }
    }
}