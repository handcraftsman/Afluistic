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

using Afluistic.Commands;

using StructureMap;
using StructureMap.Graph;

namespace Afluistic
{
    public static class IoC
    {
        public static TType Get<TType>()
        {
            return ObjectFactory.GetInstance<TType>();
        }

        public static IEnumerable<TInterface> GetAll<TInterface>()
        {
            return ObjectFactory.GetAllInstances<TInterface>();
        }

        public static void Initialize()
        {
            ObjectFactory.Initialize(x => x.Scan(s =>
            {
                s.TheCallingAssembly();
                s.AddAllTypesOf<ICommand>();
                s.WithDefaultConventions();
            }));
        }

        public static void EjectAllInstancesOf<T>()
        {
            ObjectFactory.EjectAllInstancesOf<T>();
        }
    }
}