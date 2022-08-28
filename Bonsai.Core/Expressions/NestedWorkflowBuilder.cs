﻿using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reactive.Linq;
using System;

namespace Bonsai.Expressions
{
    /// <summary>
    /// Represents an expression builder that generates an expression tree using a nested
    /// expression builder workflow.
    /// </summary>
    [XmlType("NestedWorkflow", Namespace = Constants.XmlNamespace)]
    [Description("Encapsulates complex workflow logic into a new build context.")]
    public class NestedWorkflowBuilder : WorkflowExpressionBuilder
    {
        static readonly MethodInfo deferMethod = typeof(Observable).GetMethods()
                                                                   .Single(m => m.Name == "Defer" &&
                                                                                m.GetParameters()[0].ParameterType
                                                                                 .GetGenericArguments()[0]
                                                                                 .GetGenericTypeDefinition() == typeof(IObservable<>));

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedWorkflowBuilder"/> class.
        /// </summary>
        public NestedWorkflowBuilder()
            : this(new ExpressionBuilderGraph())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedWorkflowBuilder"/> class
        /// with the specified expression builder workflow.
        /// </summary>
        /// <param name="workflow">
        /// The expression builder workflow instance that will be used by this builder
        /// to generate the output expression tree.
        /// </param>
        public NestedWorkflowBuilder(ExpressionBuilderGraph workflow)
            : base(workflow)
        {
        }

        /// <summary>
        /// Generates an <see cref="Expression"/> node from a collection of input arguments.
        /// The result can be chained with other builders in a workflow.
        /// </summary>
        /// <param name="arguments">
        /// A collection of <see cref="Expression"/> nodes that represents the input arguments.
        /// </param>
        /// <returns>An <see cref="Expression"/> tree node.</returns>
        public override Expression Build(IEnumerable<Expression> arguments)
        {
            return BuildWorkflow(arguments, null, selectorBody =>
            {
                var factory = Expression.Lambda(selectorBody);
                var resultType = selectorBody.Type.GetGenericArguments()[0];
                return Expression.Call(deferMethod.MakeGenericMethod(resultType), factory);
            });
        }
    }
}
