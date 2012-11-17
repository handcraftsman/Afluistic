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

using FluentAssert;

using NUnit.Framework;

namespace Afluistic.Tests.Domain
{
    public class TaxReportingCategoryTests
    {
        public class When_asked_if_two_tax_reporting_categories_are_Equal
        {
            [TestFixture]
            public class Given_tax_reporting_categories_having_different_names
            {
                [Test]
                public void Should_return_false()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "bar"
                        };
                    alpha.Equals(beta).ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_exact_same_name
            {
                [Test]
                public void Should_return_true()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "foo"
                        };
                    alpha.Equals(beta).ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_same_name_but_different_capitalization
            {
                [Test]
                public void Should_return_true()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "FOO"
                        };
                    alpha.Equals(beta).ShouldBeTrue();
                }
            }
        }

        public class When_asked_if_two_tax_reporting_categories_are_different_using_not_equal
        {
            [TestFixture]
            public class Given_tax_reporting_categories_having_different_names
            {
                [Test]
                public void Should_return_true()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "bar"
                        };
                    (alpha != beta).ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_exact_same_name
            {
                [Test]
                public void Should_return_false()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "foo"
                        };
                    (alpha != beta).ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_same_name_but_different_capitalization
            {
                [Test]
                public void Should_return_false()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "FOO"
                        };
                    (alpha != beta).ShouldBeFalse();
                }
            }
        }

        public class When_asked_if_two_tax_reporting_categories_are_the_same_using_double_equal
        {
            [TestFixture]
            public class Given_tax_reporting_categories_having_different_names
            {
                [Test]
                public void Should_return_false()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "bar"
                        };
                    (alpha == beta).ShouldBeFalse();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_exact_same_name
            {
                [Test]
                public void Should_return_true()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "foo"
                        };
                    (alpha == beta).ShouldBeTrue();
                }
            }

            [TestFixture]
            public class Given_tax_reporting_categories_having_the_same_name_but_different_capitalization
            {
                [Test]
                public void Should_return_true()
                {
                    var alpha = new TaxReportingCategory
                        {
                            Name = "foo",
                        };
                    var beta = new TaxReportingCategory
                        {
                            Name = "FOO"
                        };
                    (alpha == beta).ShouldBeTrue();
                }
            }
        }
    }
}