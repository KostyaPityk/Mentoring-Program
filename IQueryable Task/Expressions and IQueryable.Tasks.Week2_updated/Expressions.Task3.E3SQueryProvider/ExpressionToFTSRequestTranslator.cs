using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
	public class ExpressionToFtsRequestTranslator : ExpressionVisitor
	{
		readonly StringBuilder _resultStringBuilder;

		public ExpressionToFtsRequestTranslator()
		{
			_resultStringBuilder = new StringBuilder();
		}

		public string Translate(Expression exp)
		{
			_resultStringBuilder.Append("'statements':[");
			Visit(exp);
			_resultStringBuilder.Append("]");
			return _resultStringBuilder.ToString();
		}

		#region protected methods

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable)
				&& node.Method.Name == "Where")
			{
				var predicate = node.Arguments[1];
				Visit(predicate);

				return node;
			}

			if (node.Method.Name == "Equals")
			{
				return this.CallMethod(node, "(", ")");

			}

			if (node.Method.Name == "StartsWith")
			{

				return this.CallMethod(node, "(", "*)");

			}

			if (node.Method.Name == "EndsWith")
			{

				return this.CallMethod(node, "(*", ")");

			}

			if (node.Method.Name == "Contains")
			{

				return this.CallMethod(node, "(*", "*)");

			}

			return base.VisitMethodCall(node);
		}

		private Expression CallMethod(MethodCallExpression node, string startPrefix, string endPrefix)
		{
			Visit(node.Object);
			_resultStringBuilder.Append(startPrefix);
			var predicate = node.Arguments[0];
			Visit(predicate);
			_resultStringBuilder.Append(endPrefix);
			_resultStringBuilder.Append("'}");
			return node;

		}
		protected override Expression VisitBinary(BinaryExpression node)
		{
			var leftNode = node.Left;
			var rightNode = node.Right;

			if (node.Left.NodeType == ExpressionType.Constant)
			{
				leftNode = node.Right;
				rightNode = node.Left;
			}

			switch (node.NodeType)
			{
				case ExpressionType.Equal:
					Visit(leftNode);
					_resultStringBuilder.Append("(");
					Visit(rightNode);
					_resultStringBuilder.Append(")'}");
					break;
				case ExpressionType.AndAlso:
					Visit(leftNode);
					_resultStringBuilder.Append(",");
					Visit(rightNode);
					break;
				default:
					throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
			};

			return node;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			_resultStringBuilder.Append("{'query':");
			_resultStringBuilder.Append($"'{node.Member.Name}").Append(":");

			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			_resultStringBuilder.Append(node.Value);

			return node;
		}

		#endregion
	}
}
