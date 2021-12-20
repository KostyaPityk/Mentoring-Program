/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            // todo: feel free to add your code here
            Dictionary<string, int> valuePairs = new Dictionary<string, int>
            {
                {"a", 1 },
                {"b", 2 }
            };
            Expression<Func<int, int, int>> expression = (a, b) => a + 1 + (3 - a) + (a - 1) - (b + 1) + 3 + (a -1);
            
            
            /*var convertedExp = new IncDecExpressionVisitor(valuePairs).Visit(expression);
            Console.WriteLine(convertedExp); */

            var excuteExp = new IncDecExpressionVisitor(valuePairs).VisitAndConvert(expression, string.Empty);
            var incDecRes = excuteExp.Compile().Invoke(1, 2);

            Console.WriteLine(expression);
            Console.WriteLine(excuteExp);
           
            Console.WriteLine(incDecRes);
            Console.WriteLine();


            var pEx = new ParameterVisitor(valuePairs).Visit(expression);
            Console.WriteLine(pEx);

            Console.ReadLine();
        }
    }
}
