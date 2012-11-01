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

using Afluistic.Domain;
using Afluistic.MvbaCore;
using Afluistic.Services;
using Afluistic.Tests.Services;

using NUnit.Framework;

using StructureMap;

namespace Afluistic.Tests
{
    public class IntegrationTestBase
    {
        private static bool _iocInitialized;
        protected Notification<ApplicationSettings> Settings
        {
            get
            {
                var settings = IoC.Get<ApplicationSettingsService>().Load();
                return settings;
            }
        }

        protected string StandardErrorText
        {
            get
            {
                var standardErrorText = IoC.Get<InMemorySystemService>().StandardErrorText;
                return standardErrorText;
            }
        }
        protected string StandardOutText
        {
            get
            {
                var standardOutText = IoC.Get<InMemorySystemService>().StandardOutText;
                return standardOutText;
            }
        }
        protected Notification<Statement> Statement
        {
            get
            {
                var statement = IoC.Get<StorageService>().Load();
                return statement;
            }
        }

        [SetUp]
        public void BeforeEachTest()
        {
            Before_each_test();
        }

        [TestFixtureSetUp]
        public void BeforeFirstTest()
        {
            InjectInMemoryReplacements();
            Before_first_test();
        }

        protected virtual void Before_each_test()
        {
        }

        protected virtual void Before_first_test()
        {
        }

        protected bool FileExists(string path)
        {
            return IoC.Get<InMemoryFileSystemService>().FileExists(path);
        }

        private static void InjectInMemoryReplacements()
        {
            if (!_iocInitialized)
            {
                _iocInitialized = true;
                IoC.Initialize();
                IoC.EjectAllInstancesOf<ISystemService>();
                IoC.EjectAllInstancesOf<IFileSystemService>();
                ObjectFactory.Configure(x =>
                    {
                        x.For<ISystemService>().Use<InMemorySystemService>();
                        x.For<IFileSystemService>().Use<InMemoryFileSystemService>();
                    });
            }
            else
            {
                ResetInMemoryObjects();
            }
        }

        protected static void ResetInMemoryObjects()
        {
            IoC.Get<InMemoryFileSystemService>().Reset();
            IoC.Get<InMemorySystemService>().Reset();
        }
    }
}