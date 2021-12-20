using System;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
	public class MappingGenerator
	{
		public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
		{
			var sourceParam = Expression.Parameter(typeof(TSource));

			var destinationObjProps = typeof(TDestination).GetProperties();
			var sourceObjProps = typeof(TSource).GetProperties()
				.Where(x => destinationObjProps.FirstOrDefault(d => d.Name == x.Name && d.PropertyType == x.PropertyType) != null)
				.Select(x => Expression.Bind(typeof(TDestination).GetProperty(x.Name, x.PropertyType), Expression.Property(sourceParam, x.Name)));

			var body = Expression.MemberInit(Expression.New(typeof(TDestination)), sourceObjProps);

			var mapFunction =
				Expression.Lambda<Func<TSource, TDestination>>(
					body,
					sourceParam
				);

			return new Mapper<TSource, TDestination>(mapFunction.Compile());
		}
	}
}
