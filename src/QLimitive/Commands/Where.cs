﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using FastMember;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents where command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Where<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }


    /// <summary>
    /// Gets the expression that represents the filter condition.
    /// </summary>
    private Expression<Func<T, bool>> Predicate { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Where(DbDialect dialect, Expression<Func<T, bool>> predicate)
    {
        this.Dialect = dialect;
        this.Predicate = predicate;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        if (builder.Length > 0)
            builder.AppendLine();

        builder.AppendLine("where");
        builder.Append("    ");

        var table = TableMappingInfo.Get<T>();
        var parameter = this.Predicate.Parameters[0];
        var parser = new Parser(parameter, this.Dialect, table, ref builder, ref parameters);
        parser.Visit(this.Predicate);
    }
    #endregion


    #region Analyze expression tree (private class / enum only)
    /// <summary>
    /// Provides a conditional expression analysis.
    /// </summary>
    private unsafe sealed class Parser : ExpressionVisitor
    {
        #region Fields
        private readonly ParameterExpression parameter;
        private readonly DbDialect dialect;
        private readonly TableMappingInfo table;
        private readonly void* stringBuilderPointer;
        private readonly void* bindParametersPointer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates instance.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="dialect"></param>
        /// <param name="table"></param>
        /// <param name="builder"></param>
        /// <param name="bindParameters"></param>
        public Parser(ParameterExpression parameter, DbDialect dialect, TableMappingInfo table, ref Utf16ValueStringBuilder builder, ref BindParameterCollection? bindParameters)
        {
            this.parameter = parameter;
            this.dialect = dialect;
            this.table = table;
            this.stringBuilderPointer = Unsafe.AsPointer(ref builder);
            this.bindParametersPointer = Unsafe.AsPointer(ref bindParameters);
        }
        #endregion


        #region Overrides
        /// <summary>
        /// Scans the children of <see cref="BinaryExpression"/>.
        /// </summary>
        /// <param name="expression">Target expression</param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                //--- AND/OR : Generated an element that holds the left and right
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    {
                        this.BuildAndOr(expression);
                        return expression;
                    }

                //--- Comparison operator (<, <=, >=, >, ==, !=) : Extract member name of left node and value of right node
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    {
                        #region Local Functions
                        static (Operator @operator, string memberName, object? value) getParameter(Parser parser, BinaryExpression expression)
                        {
                            //--- 'x.Hoge == value'
                            {
                                var memberName = parser.ExtractMemberName(expression.Left);
                                if (memberName is not null)
                                {
                                    var @operator = OperatorHelper.From(expression.NodeType);
                                    var value = parser.ExtractValue(expression.Right);
                                    return (@operator, memberName, value);
                                }
                            }
                            //--- 'value == x.Hoge'
                            {
                                var memberName = parser.ExtractMemberName(expression.Right);
                                if (memberName is not null)
                                {
                                    var @operator = OperatorHelper.From(expression.NodeType);
                                    @operator = OperatorHelper.Flip(@operator);
                                    var value = parser.ExtractValue(expression.Left);
                                    return (@operator, memberName, value);
                                }
                            }
                            //--- Error
                            throw new InvalidOperationException();
                        }
                        #endregion

                        var p = getParameter(this, expression);
                        this.BuildBinary(p.@operator, p.memberName, p.value);
                        return expression;
                    }
            }
            return base.VisitBinary(expression);
        }


        /// <summary>
        /// Scans the children of <see cref="MethodCallExpression"/>.
        /// </summary>
        /// <param name="expression">Target expression</param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            //--- bool Contains(T value)
            if (expression.Method.Name == nameof(Enumerable.Contains))
            {
                var t = expression.Method.DeclaringType;
                if (t == typeof(Enumerable))
                {
                    this.BuildInClause(expression, true);
                }
                else if (t?.Namespace?.StartsWith("System.Collections") ?? false)
                {
                    this.BuildInClause(expression, false);
                }
            }

            //--- default
            return base.VisitMethodCall(expression);
        }
        #endregion


        #region Helpers
        /// <summary>
        /// Builds and/or.
        /// </summary>
        /// <param name="expression"></param>
        private void BuildAndOr(BinaryExpression expression)
        {
            ref var builder = ref Unsafe.AsRef<Utf16ValueStringBuilder>(this.stringBuilderPointer);
            var @operator = OperatorHelper.From(expression.NodeType);

            //--- left
            var leftOperator = OperatorHelper.From(expression.Left.NodeType);
            if (needsBracket(@operator, leftOperator))
            {
                builder.Append('(');
                this.Visit(expression.Left);
                builder.Append(')');
            }
            else
            {
                this.Visit(expression.Left);
            }

            //--- and / or
            if (@operator == Operator.AndAlso) builder.Append(" and ");
            if (@operator == Operator.OrElse) builder.Append(" or ");

            //--- right
            var rightOperator = OperatorHelper.From(expression.Right.NodeType);
            if (needsBracket(@operator, rightOperator))
            {
                builder.Append('(');
                this.Visit(expression.Right);
                builder.Append(')');
            }
            else
            {
                this.Visit(expression.Right);
            }

            #region Local Functions
            static bool needsBracket(Operator self, Operator side)
                => (self != side) && (side == Operator.AndAlso || side == Operator.OrElse);
            #endregion
        }


        /// <summary>
        /// Builds binary.
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        private void BuildBinary(Operator @operator, string memberName, object? value)
        {
            ref var builder = ref Unsafe.AsRef<Utf16ValueStringBuilder>(this.stringBuilderPointer);
            ref var bindParameters = ref Unsafe.AsRef<BindParameterCollection>(this.bindParametersPointer);

            //--- Build sql
            var bracket = this.dialect.KeywordBracket;
            var columnName = table.ColumnByMemberName[memberName].ColumnName;
            builder.Append(bracket.Begin);
            builder.Append(columnName);
            builder.Append(bracket.End);

            switch (@operator)
            {
                case Operator.Equal:
                    if (value is null)
                    {
                        builder.Append(" is null");
                        return;
                    }
                    builder.Append(" = ");
                    break;

                case Operator.NotEqual:
                    if (value is null)
                    {
                        builder.Append(" is not null");
                        return;
                    }
                    builder.Append(" <> ");
                    break;

                case Operator.LessThan:
                    builder.Append(" < ");
                    break;

                case Operator.LessThanOrEqual:
                    builder.Append(" <= ");
                    break;

                case Operator.GreaterThan:
                    builder.Append(" > ");
                    break;

                case Operator.GreaterThanOrEqual:
                    builder.Append(" >= ");
                    break;

                case Operator.Contains:
                    builder.Append(" in ");
                    break;

                default:
                    throw new InvalidOperationException();
            }

            //--- Cache parameter
            bindParameters ??= new BindParameterCollection();
            var name = $"p{bindParameters.Count + 1}";
            bindParameters.Add(name, value);
            builder.Append(this.dialect.BindParameterPrefix);
            builder.Append(name);
        }


        /// <summary>
        /// Build in clause.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isExtensionMethod"></param>
        private void BuildInClause(MethodCallExpression expression, bool isExtensionMethod)
        {
            //--- Gets member name
            var argExpression = isExtensionMethod ? expression.Arguments[1] : expression.Arguments[0];
            var memberName = this.ExtractMemberName(argExpression);
            if (memberName is null)
                throw new InvalidOperationException();

            //--- Generates element
            //--- If there are more than 1000 in clauses, error will occur.
            var objExpression = isExtensionMethod ? expression.Arguments[0] : expression.Object;
            if (objExpression is null)
                throw new InvalidOperationException();

            var elements = this.ExtractValue(objExpression) as IEnumerable;
            if (elements is null)
                throw new InvalidOperationException();

            var source
                = elements
                .Cast<object>()
                .Chunk(this.dialect.InOperatorMaxCount)
                .ToArray();

            //--- Build sql
            ref var builder = ref Unsafe.AsRef<Utf16ValueStringBuilder>(this.stringBuilderPointer);
            if (source.Length == 0)
            {
                //--- There is no element in the in clause, it is forced to false.
                this.BuildBoolean(false);
            }
            else if (source.Length == 1)
            {
                var x = source[0];
                this.BuildBinary(Operator.Contains, memberName, x);
            }
            else
            {
                builder.Append('(');
                for (var i = 0; i < source.Length; i++)
                {
                    if (i > 0)
                        builder.Append(" or ");
                    this.BuildBinary(Operator.Contains, memberName, source[i]);
                }
                builder.Append(')');
            }
        }


        /// <summary>
        /// Builds boolean.
        /// </summary>
        /// <param name="value"></param>
        private void BuildBoolean(bool value)
        {
            ref var builder = ref Unsafe.AsRef<Utf16ValueStringBuilder>(this.stringBuilderPointer);
            var sql = value ? "1 = 1" : "1 = 0";
            builder.Append(sql);
        }


        /// <summary>
        /// Extracts the member name from the specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string? ExtractMemberName(Expression expression)
        {
            var member = ExpressionHelper.ExtractMemberExpression(expression);
            return member?.Expression == this.parameter
                ? member.Member.Name
                : null;
        }


        /// <summary>
        /// Extracts the value from the specified expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <remarks>Please use only for right node.</remarks>
        private object? ExtractValue(Expression expression)
        {
            //--- Constant
            if (expression is ConstantExpression constant)
                return constant.Value;

            //--- Creates instance
            if (expression is NewExpression @new)
            {
                var parameters = @new.Arguments.Select(this.ExtractValue).ToArray();
                return @new.Constructor?.Invoke(parameters);
            }

            //--- new T[]
            if (expression is NewArrayExpression newArray)
            {
                return newArray.Expressions.Select(this.ExtractValue).ToArray();
            }

            //--- Method call
            if (expression is MethodCallExpression methodCall)
            {
                var parameters = methodCall.Arguments.Select(this.ExtractValue).ToArray();
                var obj = methodCall.Object is null
                        ? null  // static
                        : this.ExtractValue(methodCall.Object);  // instance
                return methodCall.Method.Invoke(obj, parameters);
            }

            //--- Delegate / Lambda
            if (expression is InvocationExpression invocation)
            {
                var parameters = invocation.Arguments.Select(x => Expression.Parameter(x.Type)).ToArray();
                var arguments = invocation.Arguments.Select(this.ExtractValue).ToArray();
                var lambda = Expression.Lambda(invocation, parameters);
                var result = lambda.Compile().DynamicInvoke(arguments);
                return result;
            }

            //--- Indexer
            if (expression is BinaryExpression binary)
                if (expression.NodeType == ExpressionType.ArrayIndex)
                {
                    var array = (Array)this.ExtractValue(binary.Left)!;
                    var index = (int)this.ExtractValue(binary.Right)!;
                    return array.GetValue(index);
                }

            //--- Field / Property
            var memberNames = new List<string>();
            var temp = expression;
            while (temp is not ConstantExpression)
            {
                //--- cast
                if (temp is UnaryExpression unary)
                    if (temp.NodeType == ExpressionType.Convert)
                    {
                        temp = unary.Operand;
                        continue;
                    }

                //--- not member
                if (temp is not MemberExpression member)
                    return this.ExtractValue(temp);

                //--- static
                if (member.Expression is null)
                {
                    if (member.Member is PropertyInfo pi)
                        return pi.GetValue(null);

                    if (member.Member is FieldInfo fi)
                        return fi.GetValue(null);

                    throw new InvalidOperationException("Not field or property.");
                }

                //--- instance
                memberNames.Add(member.Member.Name);
                temp = member.Expression;
            }

            var value = ((ConstantExpression)temp).Value;
            for (int i = memberNames.Count - 1; 0 <= i; i--)
                value = ObjectAccessor.Create(value)[memberNames[i]];
            return value;
        }
        #endregion
    }


    /// <summary>
    /// Represents a conditional expression operator.
    /// </summary>
    private enum Operator : byte
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// a &amp;&amp; b
        /// </summary>
        AndAlso,

        /// <summary>
        /// a || b
        /// </summary>
        OrElse,

        /// <summary>
        /// a &lt; b
        /// </summary>
        LessThan,

        /// <summary>
        /// a &lt;= b
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// a > b
        /// </summary>
        GreaterThan,

        /// <summary>
        /// a >= b
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// a == b
        /// </summary>
        Equal,

        /// <summary>
        /// a != b
        /// </summary>
        NotEqual,

        /// <summary>
        /// Enumerable.Contains(value)
        /// </summary>
        Contains,
    }


    /// <summary>
    /// Provides <see cref="Operator"/> helper methods.
    /// </summary>
    private static class OperatorHelper
    {
        /// <summary>
        /// Converts from <see cref="ExpressionType"/> to <seealso cref="Operator"/>.
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public static Operator From(ExpressionType expressionType)
            => expressionType switch
            {
                ExpressionType.AndAlso => Operator.AndAlso,
                ExpressionType.OrElse => Operator.OrElse,
                ExpressionType.LessThan => Operator.LessThan,
                ExpressionType.LessThanOrEqual => Operator.LessThanOrEqual,
                ExpressionType.GreaterThan => Operator.GreaterThan,
                ExpressionType.GreaterThanOrEqual => Operator.GreaterThanOrEqual,
                ExpressionType.Equal => Operator.Equal,
                ExpressionType.NotEqual => Operator.NotEqual,
                _ => Operator.Unknown,
            };


        /// <summary>
        /// Inverts the specified operator.
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static Operator Flip(Operator @operator)
            => @operator switch
            {
                Operator.LessThan => Operator.GreaterThan,
                Operator.LessThanOrEqual => Operator.GreaterThanOrEqual,
                Operator.GreaterThan => Operator.LessThan,
                Operator.GreaterThanOrEqual => Operator.LessThanOrEqual,
                _ => @operator,
            };
    }
    #endregion
}
