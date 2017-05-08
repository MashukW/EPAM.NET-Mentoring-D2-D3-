using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task2.ExpressionTransformation;

namespace Module1.ExpressoonsAndIqueryable.Tests
{
    [TestClass]
    public class Task2ExpressionTransformationTests
    {
        private ExpressionTransform exprTransform;
        private Dictionary<string, object> _testConsts;

        [TestInitialize]
        public void TestInit()
        {
            _testConsts = new Dictionary<string, object>
            {
                {"param1", 2},
                {"param2", "replacedConst"}
            };

            exprTransform = new ExpressionTransform(_testConsts);
        }

        [TestMethod]
        public void VisitBinaryReplacedByIncrementOrDecrement()
        {
            // Arrange
            string expectedResultStr = "param1 => (Increment(param1) / Decrement(param1))";
            Expression<Func<int, int>> sourceExpr = param1 => (param1 + 1)/(param1 - 1);
            
            // Act
            var resultExpr = exprTransform.VisitAndConvert(sourceExpr, "");
            
            // Assert
            StringAssert.Contains(expectedResultStr, resultExpr?.ToString());
        }
    }
}
