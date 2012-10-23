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

        private static List<string> GetNames(MemberExpression memberExpression)
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