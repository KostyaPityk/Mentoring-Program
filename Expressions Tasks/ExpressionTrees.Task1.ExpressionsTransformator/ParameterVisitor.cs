using System.Linq.Expressions;
using System.Collections.Generic;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
	public class ParameterVisitor : ExpressionVisitor
	{
		private Dictionary<string, int> parameters;

		public ParameterVisitor(Dictionary<string, int> parameters)
		{
			this.parameters = parameters;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			string key = node.Name;

			return this.parameters.ContainsKey(key) ? Expression.Constant(this.parameters[key]) : base.VisitParameter(node);
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			return Expression.Lambda(Visit(node.Body));
		}
	}
}
