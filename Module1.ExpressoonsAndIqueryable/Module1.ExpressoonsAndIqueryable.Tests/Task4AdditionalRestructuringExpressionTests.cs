using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task4.AdditionalRestructuringExpression;

namespace Module1.ExpressoonsAndIqueryable.Tests
{
    [TestClass]
    public class Task4AdditionalRestructuringExpressionTests
    {
        [TestMethod]
        public void ToBoolMethodTest()
        {
            // Arrange
            Expression<Func<string, bool?>> sourceExpr = a => a.Length > 5;

            // Act
            Expression<Func<string, bool>> resultExpr = sourceExpr.ToBool();

            // Assert
            Assert.IsNotNull(resultExpr);
            StringAssert.Contains(resultExpr.ReturnType.Name, "Boolean");
        }
    }
}
