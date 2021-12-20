using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            Foo foo = new Foo
            {
                A = 1,
                B = "1",
                C = 1
            };


            var res = mapper.Map(foo);

            Assert.AreEqual(foo.A, res.A);
            Assert.AreEqual(foo.B, res.B);
            Assert.AreEqual(res.C, 0);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Bar, Foo>();

            var bar = new Bar
            {
                A = 1,
                B = "1",
                C = 1
            };


            var res = mapper.Map(bar);

            Assert.AreEqual(bar.A, res.A);
            Assert.AreEqual(bar.B, res.B);
            Assert.AreEqual(res.C, 0);
        }
    }
}
