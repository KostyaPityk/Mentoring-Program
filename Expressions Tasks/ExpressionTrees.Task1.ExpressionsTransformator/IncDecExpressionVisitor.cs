using System.Linq.Expressions;
using System.Collections.Generic;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
	public class IncDecExpressionVisitor : ExpressionVisitor
	{

		private Dictionary<string, int> parameters;

		public IncDecExpressionVisitor(Dictionary<string, int> parameters)
		{
			this.parameters = parameters;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			Expression expression = null;
			ConstantExpression constantExpression = null;

			if (node.Right.NodeType == ExpressionType.Constant && node.Left.NodeType == ExpressionType.Parameter)
			{
				expression = node.Left;
				constantExpression = node.Right as ConstantExpression;
			}
			else if (node.Right.NodeType == ExpressionType.Parameter && node.Left.NodeType == ExpressionType.Constant)
			{
				expression = node.Right;
				constantExpression = node.Left as ConstantExpression;
			}

			if (expression != null && (int)constantExpression.Value == 1)
			{
				if (node.NodeType == ExpressionType.Add)
				{
					return Expression.Increment(Visit(expression));
				}
				else if (node.NodeType == ExpressionType.Subtract)
				{
					return Expression.Decrement((Visit(expression)));
				}
			}

			return base.VisitBinary(node);
		}

		/*protected override Expression VisitParameter(ParameterExpression node)
		{
			string key = node.Name;

			return this.parameters.ContainsKey(key) ? Expression.Constant(this.parameters[key]) : base.VisitParameter(node);
		}
		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			return Expression.Lambda(Visit(node.Body));
		}*/
	}
}
