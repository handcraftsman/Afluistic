//  * **************************************************************************
//  * Copyright (c) McCreary, Veselka, Bragg & Allen, P.C.
//  * This source code is subject to terms and conditions of the MIT License.
//  * A copy of the license can be found in the License.txt file
//  * at the root of this distribution. 
//  * By using this source code in any fashion, you are agreeing to be bound by 
//  * the terms of the MIT License.
//  * You must not remove this notice from this software.
//  * **************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Afluistic.MvbaCore
{
    public static class Reflection
    {
        private static MethodCallExpression GetMethodCallExpression<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression == null)
                {
                    throw new ArgumentException(
                        "expression must be in the form: (Foo instance) => instance.Method()");
                }
                methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                if (methodCallExpression == null)
                {
                    throw new ArgumentException(
                        "expression must be in the form: (Foo instance) => instance.Method()");
                }
            }
            return methodCallExpression;
        }

        [DebuggerStepThrough]
        public static string GetMethodName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var methodCallExpression = GetMethodCallExpression(expression);
            return methodCallExpression.Method.Name;
        }

        [DebuggerStepThrough]
        public static string GetFinalPropertyName<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("expression must be in the form: x => x.Property");
            }
            var names = GetNames(memberExpression);
            return names.Last();
        }

// ReSharper disable ReturnTypeCanBeEnumerable.Local
        private static List<string> GetNames(MemberExpression memberExpression)
// ReSharper restore ReturnTypeCanBeEnumerable.Local
        {
            var names = new List<string>
			{
				memberExpression.Member.Name
			};
            while (memberExpression.Expression as MemberExpression != null)
            {
                memberExpression = (MemberExpression)memberExpression.Expression;
                names.Insert(0, memberExpression.Member.Name);
            }
            return names;
        }
    }
}